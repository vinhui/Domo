using Domo.Misc;
using Domo.Misc.Debug;
using Domo.Scripting;
using Domo.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Domo.Packaging
{
    public class PackageManager
    {
        public List<Package> packages = new List<Package>();
        private string packagePath;
        private string manifestName;
        private string scriptExtension;

        public PackageManager()
        {
            Log.Info("Initializing package manager");
            packagePath = Config.GetValue<string>("packaging", "path");
            manifestName = Config.GetValue<string>("packaging", "manifestName");
            scriptExtension = Config.GetValue<string>("packaging", "scriptExtension");
        }

        public void LoadPackages()
        {
            packages = new List<Package>();
            List<Tuple<string, Version, PackageManifest>> availablePackages = FindAvailablePackages(packagePath, manifestName);

            foreach (var package in availablePackages)
            {
                LoadPackage(availablePackages, package.Item1, package.Item2, package.Item3);
            }
        }

        public bool ResolveDependencies(List<Tuple<string, Version, PackageManifest>> availablePackages, ScriptEngine engine, PackageManifest parentManifest, Version version)
        {
            foreach (string dependency in parentManifest.dependencies)
            {
                Version vers = null;
                string name = dependency;
                if (dependency.Contains('/'))
                {
                    string[] split = dependency.Split('/');
                    if (split.Length > 2)
                    {
                        Log.Error("Dependency {0} of {1}, version {2} is in the wrong format", dependency, parentManifest.name, version);
                        return false;
                    }
                    name = split[0];

                    if (!String.IsNullOrEmpty(split[1]))
                    {
                        try
                        {
                            vers = new Version(split[1]);
                        }
                        catch
                        {
                            Log.Error("Version number of package {0}, dependency {1} is in the wrong format", parentManifest.name, name);
                            return false;
                        }
                    }
                }

                Log.Debug("Finding dependency {0}, version {1} of package {2}", name, vers, parentManifest.name);

                Package p = packages.FirstOrDefault(x => x.manifest.name == parentManifest.name && (vers != null && x.version == vers));
                if (p == null)
                {
                    var data = availablePackages.FirstOrDefault(x => x.Item3.name == name && ((vers != null) ? x.Item2 == vers : true));
                    if (data != null)
                    {
                        p = LoadPackage(availablePackages, data.Item1, data.Item2, data.Item3);
                        if (p.engine != null)
                        {
                            engine.AddReference(p.engine);
                        }
                    }
                    else
                    {
                        Log.Error("Failed to resolve dependency {0}, version {1} of package {2}, version {3}", name, (vers != null) ? vers.ToString() : "*", parentManifest.name, version);
                    }
                }
                else
                {
                    engine.AddReference(p.engine);
                }
            }
            return true;
        }

        public Package LoadPackage(List<Tuple<string, Version, PackageManifest>> availablePackages, string path, Version version, PackageManifest manifest)
        {
            if (packages.FirstOrDefault(x => x.path == path) != null)
            {
                Log.Debug("Package {0}, version {1} is already loaded, no need to load it again", manifest.name, version);
                return null;
            }

            Log.Info("Loading package {0}, version {1}", manifest.name, version);

            IEnumerable<string> scriptPaths = Directory.EnumerateFiles(path, String.Format("*.{0}", scriptExtension));

            Log.Debug("Creating new script engine for package {0}, version {1}", manifest.name, version);

            ScriptEngine engine = new ScriptEngine(path);

            if(manifest.searchPaths != null)
            {
                foreach (var searchPath in manifest.searchPaths)
                {
                    engine.AddSearchPath(searchPath);
                }
            }

            if (manifest.dependencies != null)
            {
                Log.Debug("Resolving dependencies for package {0}, version {1}", manifest.name, version);

                if (!ResolveDependencies(availablePackages, engine, manifest, version))
                {
                    Log.Error("Failed to load dependencies of package {0}, version {1}", manifest.name, version);
                    return null;
                }
            }

            Log.Debug("Compiling scripts for package {0}, version {1}", manifest.name, version);

            engine.AddPermissions(manifest.permissions);
            engine.AddFiles(scriptPaths.ToArray());

            Package p = new Package(manifest, version, path, engine);
            packages.Add(p);

            return p;
        }

        public void UnloadPackages()
        {
            foreach (Package package in packages)
            {
                package.engine.Unload();
            }
            packages = new List<Package>();
        }

        private List<Tuple<string, Version, PackageManifest>> FindAvailablePackages(string path, string manifestName)
        {
            List<Tuple<string, Version, PackageManifest>> availablePackages = new List<Tuple<string, Version, PackageManifest>>();

            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), path);
            Log.Info("Indexing all packages at {0}", absolutePath);

            if (Directory.Exists(absolutePath))
            {
                string[] directories = Directory.GetDirectories(path);
                Log.Debug("Found {0} potentional packages", directories.Length);

                foreach (string directory in directories)
                {
                    Log.Debug("Checking directory {0} for packages", directory);
                    string[] versionDirectories = Directory.GetDirectories(directory);
                    foreach (string versionDir in versionDirectories)
                    {
                        IEnumerable<string> files = Directory.EnumerateFiles(versionDir);
                        string manifestPath = files.FirstOrDefault(x => x == Path.Combine(versionDir, manifestName));

                        if (!string.IsNullOrEmpty(manifestPath))
                        {
                            string manifestContent = File.ReadAllText(manifestPath);
                            Log.Debug("Deserializing manifest at {0}", manifestPath);
                            PackageManifest manifest = Serializer.instance.Deserialize<PackageManifest>(manifestContent);

                            Log.Debug("Trying to parse version of {0}", manifest.name);
                            Version version = new Version(new DirectoryInfo(versionDir).Name);

                            Log.Debug("Found package {0}, version {1} at {2}", manifest.name, version, manifestPath);

                            availablePackages.Add(new Tuple<string, Version, PackageManifest>(versionDir, version, manifest));
                        }
                    }
                }
            }
            else
                Log.Warning("Packages directory does not exist ({0})", absolutePath);

            return availablePackages;
        }
    }
}