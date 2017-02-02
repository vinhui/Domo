using Domo.Misc;
using Domo.Misc.Debug;
using IronPython.Hosting;
using IronPython.Modules;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using IronPython.Runtime.Types;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PythonEngine = Microsoft.Scripting.Hosting.ScriptEngine;

namespace Domo.Scripting
{
    public class ScriptEngine
    {
        public PythonEngine engine { get; private set; }
        private ScriptScope _scope;
        public ScriptScope scope { get { return _scope; } }

        public ScriptEngine()
        {
            engine = CreatePythonEngine(out _scope);
        }

        public void AddReference(ScriptEngine engine)
        {
            Log.Debug("Adding reference to a different script engine");
            var vars = engine.GetVariables();
            foreach (var item in vars)
            {
                if (item.Value != null && !item.Key.StartsWith("__") && !ContainsVar(item.Key))
                {
                    Log.Debug("Setting var {0} to {1}", item.Key, item.Value);
                    scope.SetVariable(item.Key, item.Value);
                }
            }
        }

        public bool ContainsVar(string name)
        {
            try
            {
                scope.GetVariable(name);
            }
            catch (MissingMemberException)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<KeyValuePair<string, dynamic>> GetVariables()
        {
            return scope.GetItems();
        }

        public IEnumerable<PythonType> GetTypes<T>()
        {
            return GetTypes(typeof(T));
        }

        public IEnumerable<PythonType> GetTypes(Type t)
        {
            List<PythonType> types = new List<PythonType>();

            foreach (var item in GetTypes())
            {
                if (PythonOps.IsSubClass(item, DynamicHelpers.GetPythonTypeFromType(t)))
                {
                    types.Add(item);
                }
            }

            return types;
        }

        public IEnumerable<PythonType> GetTypes()
        {
            List<PythonType> types = new List<PythonType>();

            foreach (var item in scope.GetItems())
            {
                if (item.Value is PythonType)
                {
                    PythonType type = item.Value;

                    Type t = ClrModule.GetClrType(type);

                    if (!t.IsAbstract)
                    {
                        types.Add(type);
                    }
                }
            }

            return types;
        }

        public void AddFile(string path)
        {
            Log.Debug("Loading script '{0}'", path);
            ScriptSource source = engine.CreateScriptSourceFromFile(path, Encoding.Default, SourceCodeKind.File);

            try
            {
                CompiledCode compiled = source.Compile();
                compiled.Execute(scope);
            }
            catch (SyntaxErrorException ex)
            {
                Log.Error("Failed to load {0}, error on line {1} and column {2}: {3}", Path.GetFileNameWithoutExtension(path), ex.Line, ex.Column, ex.Message);
                Log.Error("Line: '{0}'", source.GetCodeLine(ex.Line));
            }
            catch (IronPython.Runtime.Exceptions.ImportException ex)
            {
                Log.Error("Failed to load {0}, there was an error with importing a module", Path.GetFileNameWithoutExtension(path));
                Log.Exception(ex);
            }
        }

        public void AddFiles(params string[] paths)
        {
            foreach (string path in paths)
            {
                AddFile(path);
            }
        }

        private PythonEngine CreatePythonEngine(out ScriptScope globalScope)
        {
            PythonEngine e;

            if (Config.GetValue<bool>("python", "debug"))
            {
                Log.Debug("Creating new python engine with debugging enabled");
                Dictionary<string, object> options = new Dictionary<string, object>();
                options["Debug"] = true;
                e = Python.CreateEngine(options);
            }
            else
            {
                Log.Debug("Creating new python engine");
                e = Python.CreateEngine();
            }

            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                e.Runtime.LoadAssembly(item);
            }
            globalScope = e.CreateScope();

            PythonType log = DynamicHelpers.GetPythonTypeFromType(typeof(Log));
            globalScope.SetVariable("log", log);
            globalScope.SetVariable("Log", log);

            ScriptScope builtinScope = Python.GetBuiltinModule(e);
            //builtinScope.SetVariable("__import__", new Func<CodeContext, string, PythonDictionary, PythonDictionary, PythonTuple, object>(ImportModule));

            return e;
        }

        private static object ImportModule(CodeContext context, string moduleName, PythonDictionary globals, PythonDictionary locals, PythonTuple tuple)
        {
            object o = Builtin.__import__(context, moduleName, globals, locals, tuple, -1);
            return o;
        }

        public void Unload()
        {
        }
    }
}