using Domo.Misc.Debug;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using IronPython.Runtime.Types;
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
        private Dictionary<PythonType, ModuleBase> modules = new Dictionary<PythonType, ModuleBase>();

        /// <summary>
        /// Load all the modules from a script scope
        /// </summary>
        /// <param name="scope">Scope to get modules from</param>
        /// <exception cref="ArgumentException">Gets thrown when the type is already loaded</exception>
        public void LoadModules(IEnumerable<Scripting.ScriptEngine> engines)
        {
            List<PythonType> types = new List<PythonType>();

            foreach (var engine in engines)
            {
                foreach (var type in engine.GetTypes<ModuleBase>())
                {
                    Log.Debug("Found module '{0}'", PythonType.Get__name__(type));
                    modules.Add(
                        type,
                        engine.engine.Operations.CreateInstance(type) as ModuleBase
                        );
                }
            }

            foreach (var item in modules)
            {
                FixReferences(item.Value);
            }

            Log.Info("Found a total of {0} modules", modules.Count);

            // All the references should be assigned before going into the OnEnable
            foreach (var item in modules)
            {
                item.Value.OnEnable();
            }

            Log.Debug("Called all OnEnable methods on the modules");
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
            foreach (var item in modules)
            {
                if (item.Key.__clrtype__() == t)
                    return item.Value;
            }

            throw new KeyNotFoundException("The module you're trying to get an instance of has not been loaded");
        }

        /// <summary>
        /// Goes through all the properties in the module and fixes all the properties with the <see cref="AutoFillGenericAttribute"/> attribute
        /// </summary>
        /// <param name="module">Module to fix the references for</param>
        private void FixReferences(ModuleBase module)
        {
            Type type = module.GetType();
            FieldInfo[] properties = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            foreach (var item in properties)
            {
                if (item.IsDefined(typeof(AutoFillGenericAttribute), true))
                {
                    Type ownerType = item.DeclaringType;
                    if (true)
                    {
                        // Make sure there is something to assign the property with
                        if (ownerType.GenericTypeArguments.Length > 0)
                        {
                            Type propType = item.FieldType;

                            // Bool that gets set when the property is sucessfully set, this is for logging purposes
                            bool set = false;
                            foreach (var generic in ownerType.GenericTypeArguments)
                            {
                                if (generic == propType)
                                {
                                    item.SetValue(module, GetInstance(propType));
                                    set = true;
                                    break;
                                }
                            }
                            if (!set)
                                Log.Warning("Property '{0}' in '{1}' has the '{2}' attribute but is not of the same type as one of the class generics", item.Name, ownerType.Name, typeof(AutoFillGenericAttribute).Name);
                        }
                        else
                            Log.Warning("Property '{0}' in '{1}' has the '{2}' attribute but is not in a generic class!", item.Name, ownerType.Name, typeof(AutoFillGenericAttribute).Name);
                    }
                    else
                        Log.Warning("Property '{0}' in '{1}' has the '{2}' attribute but does not have a setter!", item.Name, ownerType.Name, typeof(AutoFillGenericAttribute).Name);
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
            Log.Debug("Disposing of the module factory");
            foreach (var item in modules)
            {
                item.Value.OnDisable();
            }
            modules.Clear();
        }
    }
}