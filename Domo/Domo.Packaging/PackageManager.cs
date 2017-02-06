using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Domo.Misc;
using Domo.Serialization;
using Domo.Misc.Debug;
using Domo.Scripting;

namespace Domo.Packaging
{
    public class PackageManager
    {
        public List<Package> packages = new List<Package>();
        string packagePath;
        string manifestName;
        string scriptExtension;

        public PackageManager()
        {
            Log.Debug("Initializing package manager");
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

        public bool ResolveDependencies(List<Tuple<string, Version, PackageManifest>> availablePackages, ScriptEngine engine, PackageManifest manifest, Version version)
        {
            foreach (string dependency in manifest.dependencies)
            {
                if (dependency.Contains('\\'))
                {
                    string[] split = dependency.Split('\\');
                    if (split.Length > 2)
                    {
                        Log.Error("Dependency {0} of {1}, version {2} is in the wrong format", dependency, manifest.name, manifest, version);
                        return false;
                    }
                    string name = split[0];
                    Version vers = null;

                    try
                    {
                        vers = new Version(split[1]);
                    }
                    catch
                    {
                        Log.Error("Version number of package {0}, dependency {1} is in the wrong format", manifest.name, name);
                        return false;
                    }

                    if (vers != null)
                    {
                        Log.Debug("Finding dependency {0}, version {1} of package {2}", name, vers, manifest.name);

                        Package p = packages.FirstOrDefault(x => x.manifest.name == manifest.name && x.version == version);
                        if (p == null)
                        {
                            var data = availablePackages.FirstOrDefault(x => x.Item3.name == name && x.Item2 == version);
                            if (data != null)
                            {
                                p = LoadPackage(availablePackages, data.Item1, data.Item2, data.Item3);
                                engine.AddReference(p.engine);
                            }
                        }
                        else
                        {
                            engine.AddReference(p.engine);
                        }
                    }
                }
            }
            return true;
        }

        public Package LoadPackage(List<Tuple<string, Version, PackageManifest>> availablePackages, string path, Version version, PackageManifest manifest)
        {
            if (packages.FirstOrDefault(x => x.path == path) != null)
            {
                Log.Debug("Package {0}, version {1} is already loaded", manifest.name, version);
                return null;
            }

            Log.Info("Loading package {0}", manifest.name);

            IEnumerable<string> scriptPaths = Directory.EnumerateFiles(path, String.Format("*.{0}", scriptExtension));

            Log.Debug("Creating new script engine for package {0}, version {1}", manifest.name, version);

            ScriptEngine engine = new ScriptEngine();

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

            path = Path.Combine(Directory.GetCurrentDirectory(), path);
            Log.Debug("Indexing all packages at {0}", path);
            string[] directories = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), path));
            Log.Debug("Found {0} potentional packages", directories.Length);
            foreach (string directory in directories)
            {
                string[] versionDirectories = Directory.GetDirectories(directory);
                foreach (string versionDir in versionDirectories)
                {
                    IEnumerable<string> files = Directory.EnumerateFiles(versionDir);
                    string manifestPath = files.FirstOrDefault(x => x == Path.Combine(versionDir, manifestName));

                    if (!string.IsNullOrEmpty(manifestPath))
                    {
                        string manifestContent = File.ReadAllText(manifestPath);
                        PackageManifest manifest = Serializer.instance.Deserialize<PackageManifest>(manifestContent);

                        Version version = new Version(new DirectoryInfo(versionDir).Name);

                        Log.Debug("Found package {0}, version {1} at {2}", manifest.name, version, manifestPath);

                        availablePackages.Add(new Tuple<string, Version, PackageManifest>(versionDir, version, manifest));
                    }
                }
            }

            return availablePackages;
        }
    }
}
