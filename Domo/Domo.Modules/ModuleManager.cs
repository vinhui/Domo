using Domo.Misc.Debug;
using Domo.Packaging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            Log.Info("Loading all assemblies in the current app domain");
            Stopwatch s = Stopwatch.StartNew();

            factory.LoadModules(packages.Select(x => x.engine.scope));

            s.Stop();
            Log.Info("Finished loading all assemblies in {0}ms", s.ElapsedMilliseconds);
        }

        /// <summary>
        /// Get a reference to a module of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to get a reference of</typeparam>
        /// <returns>Returns an instance</returns>
        public static T GetModuleReference<T>() where T : ModuleBase
        {
            return factory.GetInstance<T>();
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