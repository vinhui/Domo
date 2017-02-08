using Domo.Misc.Debug;
using Domo.Packaging;
using IronPython.Runtime.Types;
using System.Collections.Generic;
using System.Diagnostics;

namespace Domo.Modules
{
    /// <summary>
    /// Class responsibe for all the module related things
    /// </summary>
    public static class ModuleManager
    {
        private static ModuleFactory factory;

        /// <summary>
        /// Initialize the manager
        /// </summary>
        public static void Init()
        {
            if (factory != null)
                Log.Warning("ModuleManager has already been initialized, this can result in strange behaviour!");

            factory = new ModuleFactory();
        }

        /// <summary>
        /// Load all the modules
        /// </summary>
        public static void LoadAllModules(IEnumerable<Package> packages)
        {
            Log.Info("Loading all modules from the currently loaded packages");
            Stopwatch s = Stopwatch.StartNew();

            factory.LoadModules(packages);

            s.Stop();
            Log.Info("Finished loading all modules in {0}ms", s.ElapsedMilliseconds);
        }

        /// <summary>
        /// Get a reference to a module of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to get a reference of</typeparam>
        /// <returns>Returns an instance</returns>
        public static ModuleBase GetModuleReference(PythonType type)
        {
            return factory.GetInstance(type);
        }

        /// <summary>
        /// Properly dispose of all the modules
        /// </summary>
        public static void ShutDown()
        {
            factory.Dispose();
        }
    }
}