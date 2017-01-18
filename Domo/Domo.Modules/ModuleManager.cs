using Domo.Misc.Debug;
using System;
using System.Reflection;

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
        /// Load all the modules in the current appdomain
        /// </summary>
        public static void LoadAllModules()
        {
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                LoadModules(item);
            }
        }

        /// <summary>
        /// Load modules from a single assembly
        /// </summary>
        /// <param name="assembly">Assembly to load modules from</param>
        public static void LoadModules(Assembly assembly)
        {
            factory.LoadModules(assembly);
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