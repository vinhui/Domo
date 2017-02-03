using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;

namespace TestRunner
{
    class Program {
        private bool _verbose, _runLongRunning, _admin, _quiet;
        private int _threadCount = 1;
        private List<TestResult> _results = new List<TestResult>();

        static int Main(string[] args) {
            return new Program().MainBody(args);
        }

        int MainBody(string[] args) {
            if (args.Length == 0) {
                Help();
                return -1;
            }

            // try and define DLR_ROOT if not already defined (makes it easier to debug TestRunner in an IDE)
            string dlrRoot = Environment.GetEnvironmentVariable("DLR_ROOT");
            if (dlrRoot == null) {
                dlrRoot = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..\\..\\..\\..\\..");
                Environment.SetEnvironmentVariable("DLR_ROOT", dlrRoot);
            }

            // Parse the options
            List<string> inputFiles = new List<string>();
            List<string> categories = new List<string>();
            List<string> tests = new List<string>();
            bool runAll = false;
            string binPath = Path.Combine(dlrRoot, "bin\\Debug"); // Default to debug binaries
            string nunitOutputPath = null;

            for (int i = 0; i < args.Length; i++) {
                if (args[i].StartsWith("/category:")) {
                    categories.Add(args[i].Substring("/category:".Length));
                } else if (args[i].StartsWith("/test:")) {
                    tests.Add(args[i].Substring("/test:".Length));
                } else if (args[i].StartsWith("/binpath:")) {
                    binPath = Path.Combine(dlrRoot, args[i].Substring("/binpath:".Length));
                }
                else if (args[i].StartsWith("/nunitoutput:"))
                {
                    nunitOutputPath = args[i].Substring("/nunitoutput:".Length);
                } else if (args[i] == "/verbose") {
                    _verbose = true;
                } else if (args[i] == "/runlong") {
                    _runLongRunning = true;
                } else if (args[i] == "/admin") {
                    _admin = true;
                } else if (args[i] == "/quiet") {
                    _quiet = true;
                } else if (args[i].StartsWith("/threads:")) {
                    int threadCount;
                    if (!Int32.TryParse(args[i].Substring("/threads:".Length), out threadCount) || threadCount <= 0) {
                        Console.WriteLine("Bad thread count: {0}", args[i].Substring("/threads:".Length));
                        return -1;
                    }
                    _threadCount = threadCount; 
                } else if (args[i] == "/all") {
                    runAll = true;
                } else if(File.Exists(args[i])) {
                    inputFiles.Add(args[i]);
                } else {
                    Console.WriteLine("Unknown option: {0}", args[i]);
                    Help();
                    return -1;
                }
            }

            // Read the test list
            XmlSerializer serializer = new XmlSerializer(typeof(TestList));
            List<TestList> testLists = new List<TestList>();
            foreach (var file in inputFiles) {
                using (var fs = new FileStream(args[0], FileMode.Open, FileAccess.Read)) {
                    testLists.Add((TestList)serializer.Deserialize(fs));
                }
            }

            if (!runAll && categories.Count == 0 && tests.Count == 0) {
                Console.WriteLine("Available categories: ");
                foreach (var list in testLists) {
                    foreach (var cat in list.Categories) {
                        Console.WriteLine("    " + cat.Name);
                    }

                    Console.WriteLine("Use /all to run all tests, /category:pattern to run categories, /test:pattern");
                    Console.WriteLine("to run individual tests. Multiple category and test options can be combined.");
                }
                return -1;
            }

            // Filter the test list
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;

            var testNamesRegex = CompileGlobPatternsToRegex(tests);
            var categoryNamesRegex = CompileGlobPatternsToRegex(categories, startsWith: true);

            List<Test> testList = new List<Test>();
            List<Test> notParallelSafe = new List<Test>();
            foreach (var list in testLists) {
                foreach (var cat in list.Categories) {
                    if (runAll || categoryNamesRegex.IsMatch(cat.Name)) {
                        foreach (var test in cat.Tests) {
                            test.Category = cat;

                            if (runAll || testNamesRegex.IsMatch(test.Name)) {
                                if (test.NotParallelSafe) {
                                    notParallelSafe.Add(test);
                                } else {
                                    testList.Add(test);
                                }
                            }
                        }
                    }
                }
            }

            // Set DLR_BIN, some tests expect this to be defined.
            Environment.SetEnvironmentVariable("DLR_BIN", binPath);

            // start the test running threads
            DateTime start = DateTime.Now;

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < _threadCount; i++) {
                Thread t = new Thread(() => {
                    Test curTest;

                    for(;;) {
                        lock (testList) {
                            if (testList.Count == 0) {
                                break;
                            }

                            curTest = testList[testList.Count - 1];
                            testList.RemoveAt(testList.Count - 1);
                        }

                        RunTestForConsole(curTest);
                    }
                });
                t.Start();
                threads.Add(t);
            }
            
            foreach (var thread in threads) {
                thread.Join();
            }

            foreach (var test in notParallelSafe) {
                RunTestForConsole(test);
            }

            var failures = _results.Where(r => r.IsFailure).ToList();

            if (failures.Count > 0) {
                if (_verbose) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Failed test output:");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    foreach (var failedTest in failures) {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(failedTest.Test.Name);
                        Console.ForegroundColor = ConsoleColor.Gray;

                        foreach (var outputLine in failedTest.Output) {
                            Console.WriteLine(outputLine);
                        }
                    }
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed test summary:");
                Console.ForegroundColor = ConsoleColor.Gray;
                foreach (var failedTest in failures) {
                    Console.WriteLine(failedTest.Test.Name);
                }
            }

            var elapsedTime = (DateTime.Now - start);

            if (!string.IsNullOrWhiteSpace(nunitOutputPath))
            {
                var resultsWriter = new NUnitResultsWriter(_results, inputFiles.First(), elapsedTime);
                resultsWriter.Save(nunitOutputPath);
            }

            Console.WriteLine("Total time: {0} seconds", elapsedTime.TotalSeconds);
            Console.ForegroundColor = originalColor;

            return failures.Count;
        }

        private void RunTestForConsole(Test test) {
            lock (this) {
                if (!_quiet && _verbose) {
                    Console.Write("{0,-100}", test.Category.Name + " " + test.Name);
                }
            }

            TestResult result = null;
            try {
                result = RunTest(test);
            } catch (Exception e) {
                result = new TestResult(test, TestResultStatus.Failed, 0, new List<string> { e.ToString() });
            }

            lock (this) {
                if (!_quiet) {
                    if (_verbose) {
                        const string resultFormat = "{0,-10}";
                        var originalColor = Console.ForegroundColor;
                        switch (result.Status) {
                            case TestResultStatus.Skipped:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write(resultFormat, "SKIPPED");
                                break;
                            case TestResultStatus.TimedOut:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(resultFormat, "TIMEOUT");
                                break;
                            case TestResultStatus.Passed:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(resultFormat, "PASSED");
                                break;
                            case TestResultStatus.Failed:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(resultFormat, "FAILED");
                                break;
                            case TestResultStatus.Disabled:
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write(resultFormat, "DISABLED");
                                break;
                        }
                        Console.ForegroundColor = originalColor;
                        Console.WriteLine(result.EllapsedSeconds);
                    } else {
                        if (result.Status == TestResultStatus.Passed) {
                            Console.Write(".");
                        } else {
                            Console.Write(result.Status.ToString()[0]);
                        }
                    }
                }

                if (result.IsFailure) {
                    DisplayFailure(test, result);
                }

                _results.Add(result);
            }
        }

        private void DisplayFailure(Test test, TestResult result) {
            if (_verbose && !_quiet) {
                Console.WriteLine("Repro:");
                if (test.EnvironmentVariables != null) {
                    foreach (var envVar in test.EnvironmentVariables) {
                        Console.WriteLine("SET {0}={1}", envVar.Name, envVar.Value);
                    }
                }
                Console.WriteLine("CD /D {0}", test.WorkingDirectory);
                Console.WriteLine("{0} {1}", test.Filename, test.Arguments);

                Console.WriteLine();
                Console.WriteLine("Result: ");
                foreach (var line in result.Output) {
                    Console.WriteLine("> {0}", line);
                }
            }
        }

        /// <summary>
        /// Runs a single test caseand returns the result.
        /// </summary>
        private TestResult RunTest(Test test) {
            if (test.Disabled) {
                return new TestResult(test, TestResultStatus.Disabled, 0, null);
            } else if ((test.LongRunning && !_runLongRunning) || (test.RequiresAdmin && !_admin)) {
                return new TestResult(test, TestResultStatus.Skipped, 0, null);
            }

            // start the process
            DateTime startTime = DateTime.Now;
            Process process = null;
            try {
                process = Process.Start(CreateProcessInfoFromTest(test));
            } catch (Win32Exception e) {
                return new TestResult(test, TestResultStatus.Failed, 0, new List<string> { e.Message });
            }
            
            // get the output asynchronously
            List<string> output = new List<string>();
            process.OutputDataReceived += (sender, e) => {
                var line = e.Data;
                if (line != null) {
                    lock (output) {
                        output.Add(line);
                    }
                }
            };
            process.BeginOutputReadLine();


            process.ErrorDataReceived += (sender, e) => {
                var line = e.Data;
                if (line != null) {
                    lock (output) {
                        output.Add(line);
                    }
                }
            };
            process.BeginErrorReadLine();

            // wait for it to exit
            if (test.MaxDuration > 0) {
                process.WaitForExit(test.MaxDuration);
            } else {
                process.WaitForExit();
            }

            process.CancelOutputRead();
            process.CancelErrorRead();

            // kill if it needed, save status
            TestResultStatus status;
            if (!process.HasExited) {
                status = TestResultStatus.TimedOut;
                process.Kill();
            } else if (process.ExitCode == 0) {
                status = TestResultStatus.Passed;
            } else {
                status = TestResultStatus.Failed;
            }

            return new TestResult(test, status, (DateTime.Now - startTime).TotalSeconds, output);
        }

        private static ProcessStartInfo CreateProcessInfoFromTest(Test test) {
            ProcessStartInfo psi = new ProcessStartInfo();
            var args = test.Arguments.Contains("-X:Debug") ? test.Arguments : "-X:Debug " + test.Arguments;
            psi.Arguments = Environment.ExpandEnvironmentVariables(test.Arguments);
            psi.WorkingDirectory = Environment.ExpandEnvironmentVariables(test.WorkingDirectory);
            psi.FileName = Environment.ExpandEnvironmentVariables(test.Filename);
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;

            if (test.EnvironmentVariables != null) {
                foreach (var envVar in test.EnvironmentVariables) {
                    psi.EnvironmentVariables.Add(envVar.Name, envVar.Value);
                }
            }
            return psi;
        }

        private static Regex CompileGlobPatternsToRegex(IEnumerable<string> patterns, bool startsWith = false) {
            var sb = new StringBuilder();

            foreach (var p in patterns) {
                if (sb.Length > 0)
                    sb.Append("|");

                sb.AppendFormat(
                    "(\\A{0}{1})",
                    Regex.Escape(p).Replace("\\*", ".*").Replace("\\?", "."),
                    startsWith ? "" : "\\Z");
            }

            return new Regex(sb.ToString(), RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        private static void Help() {
            Console.WriteLine("Usage: ");
            Console.WriteLine("TestRunner.exe (inputFile ...) [/threads:6] [/quiet] [/admin] [/binpath:dir] [/runlong] [/verbose] [/nunitoutput:file] (/all | ([/category:pattern] | [/test:pattern])+)");
        }
    }
}
