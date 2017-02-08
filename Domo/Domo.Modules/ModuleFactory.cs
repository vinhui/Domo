using Domo.Misc.Debug;
using Domo.Packaging;
using IronPython.Runtime.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domo.Modules
{
    /// <summary>
    /// Class responsibe for loading and keeping references to all the modules
    /// </summary>
    public class ModuleFactory : IDisposable
    {
        private List<ModuleListItem> modules = new List<ModuleListItem>();

        /// <summary>
        /// Load all the modules from a script scope
        /// </summary>
        /// <param name="scope">Scope to get modules from</param>
        /// <exception cref="ArgumentException">Gets thrown when the type is already loaded</exception>
        public void LoadModules(IEnumerable<Package> packages)
        {
            foreach (var package in packages)
            {
                IEnumerable<PythonType> types = SortTypes(package.engine.GetTypes<ModuleBase>(), package.manifest.executionPriority);

                foreach (var type in types)
                {
                    Log.Debug("Found module '{0}'", PythonType.Get__name__(type));
                    modules.Add(new ModuleListItem(
                        package,
                        type,
                        package.engine.engine.Operations.CreateInstance(type) as ModuleBase
                        ));
                }
            }

            Log.Info("Found a total of {0} modules", modules.Count);

            // All the references should be assigned before going into the OnEnable
            foreach (var item in modules)
            {
                Log.Debug("Calling OnEnable on '{0}'", item.name);
                item.instance.OnEnable();
            }

            Log.Debug("Called all OnEnable methods on the modules");
        }

        private IEnumerable<PythonType> SortTypes(IEnumerable<PythonType> types, string[] order)
        {
            if (order == null || order.Length == 0)
                return types;

            List<PythonType> typeList = new List<PythonType>(types);
            List<string> typeNames = typeList.Select(x => PythonType.Get__name__(x)).ToList();

            for (int i = order.Length - 1; i >= 0; i--)
            {
                for (int n = 0; n < typeNames.Count; n++)
                {
                    if (order[i] == typeNames[n])
                    {
                        PythonType t = typeList[n];
                        string name = typeNames[n];
                        typeList.RemoveAt(n);
                        typeNames.RemoveAt(n);
                        typeList.Insert(0, t);
                        typeNames.Insert(0, name);
                        break;
                    }
                }
            }

            return typeList;
        }

        /// <summary>
        /// Get an instance of a module
        /// </summary>
        /// <param name="t">Type of module to get an instance from</param>
        /// <returns>Returns the instance</returns>
        public ModuleBase GetInstance(PythonType t)
        {
            foreach (var item in modules)
            {
                if (item.type == t)
                    return item.instance;
            }

            throw new KeyNotFoundException("The module you're trying to get an instance of has not been loaded");
        }

        private bool CheckType(Type t, bool throwException = true)
        {
            if (!t.IsSubclassOf(typeof(ModuleBase)))
            {
                if (throwException)
                    throw new TypeLoadException(string.Format("The type of module you're trying to get an instance ({0}) of does not derive of '{1}'", t.FullName, typeof(ModuleBase).FullName));
                return false;
            }
            else if (t.IsAbstract)
            {
                if (throwException)
                    throw new TypeLoadException(string.Format("The module of type '{0}' that you're trying to get an instance of is abstract", t.FullName));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calls the OnDisable of all the modules and clears the list of modules
        /// </summary>
        public void Dispose()
        {
            Log.Info("Disposing of the module factory");
            modules.Reverse();
            foreach (var item in modules)
            {
                Log.Debug("Calling OnDisable on '{0}'", item.name);
                item.instance.OnDisable();
            }
            modules.Clear();
        }

        private class ModuleListItem
        {
            public string name { get { return PythonType.Get__name__(type); } }
            public readonly Package package;
            public readonly PythonType type;
            public readonly ModuleBase instance;

            public ModuleListItem(Package package, PythonType type, ModuleBase instance)
            {
                this.package = package;
                this.type = type;
                this.instance = instance;
            }
        }
    }
}