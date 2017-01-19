using Domo.Misc.Debug;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Domo.Modules
{
    /// <summary>
    /// Class responsibe for loading and keeping references to all the modules
    /// </summary>
    public class ModuleFactory : IDisposable
    {
        private Dictionary<Type, ModuleBase> modules = new Dictionary<Type, ModuleBase>();

        /// <summary>
        /// Load all the modules from an assembly
        /// </summary>
        /// <param name="assembly">Assembly to load the modules from</param>
        /// <exception cref="ArgumentException">Gets thrown when the type is already loaded</exception>
        public void LoadModules(Assembly assembly)
        {
            foreach (var item in assembly.GetTypes())
            {
                if (CheckType(item, false))
                {
                    ModuleBase module = (ModuleBase)Activator.CreateInstance(item);
                    modules.Add(item, module);
                }
            }

            foreach (var item in modules)
            {
                FixReferences(item.Value);
            }

            // All the references should be assigned before going into the OnEnable
            foreach (var item in modules)
            {
                item.Value.OnEnable();
            }
        }

        /// <summary>
        /// Get an instance of a module
        /// </summary>
        /// <typeparam name="T">Type of module to get an instance from</typeparam>
        /// <returns>Returns the instance</returns>
        public T GetInstance<T>() where T : ModuleBase
        {
            return (T)GetInstance(typeof(T));
        }

        /// <summary>
        /// Get an instance of a module
        /// </summary>
        /// <param name="t">Type of module to get an instance from</param>
        /// <returns>Returns the instance</returns>
        public ModuleBase GetInstance(Type t)
        {
            CheckType(t);

            if (modules.ContainsKey(t))
                return modules[t];
            else
                throw new KeyNotFoundException("The module you're trying to get an instance of has not been loaded");
        }

        /// <summary>
        /// Goes through all the properties in the module and fixes all the properties with the <see cref="AutoFillGenericAttribute"/> attribute
        /// </summary>
        /// <param name="module">Module to fix the references for</param>
        private void FixReferences(ModuleBase module)
        {
            Type type = module.GetType();
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            foreach (var item in properties)
            {
                if (item.IsDefined(typeof(AutoFillGenericAttribute), true))
                {
                    // Make sure there is something to assign the property with
                    Type parentType = item.DeclaringType;
                    if (parentType.ContainsGenericParameters)
                    {
                        Type propType = item.PropertyType;

                        // Bool that gets set when the property is sucessfully set, this is for logging purposes
                        bool set = false;
                        foreach (var generic in parentType.GenericTypeArguments)
                        {
                            if (generic == propType)
                            {
                                item.SetValue(module, GetInstance(propType));
                                set = true;
                                break;
                            }
                        }
                        if (!set)
                            Log.Warning("Property '{0}' in '{1}' has the '{2}' attribute but is not of the same type as one of the class generics", item.Name, parentType.FullName, typeof(AutoFillGenericAttribute).Name);
                    }
                    else
                        Log.Warning("Property '{0}' in '{1}' has the '{2}' attribute but is not in a generic class!", item.Name, parentType.FullName, typeof(AutoFillGenericAttribute).Name);
                }
            }
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
            foreach (var item in modules)
            {
                item.Value.OnDisable();
            }
            modules.Clear();
        }
    }
}