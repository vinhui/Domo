using Domo.Misc.Debug;
using IronPython.Runtime.Types;

namespace Domo.Modules
{
    public abstract class ModuleBase
    {
        public T GetModuleReference<T>(PythonType t) where T : class
        {
            ModuleBase reference = ModuleManager.GetModuleReference(t);
            if (reference is T)
                return reference as T;
            else
            {
                Log.Error("Failed to fix reference for '{0}', types do not match (requested type: '{1}'; actual type '{2}')", PythonType.Get__name__(t), typeof(T), reference.GetType());
                return null;
            }
        }

        /// <summary>
        /// Gets called when the program starts or the module gets enabled
        /// </summary>
        public virtual void OnEnable() { }

        /// <summary>
        /// Gets called when the program shuts down or when the module gets disabled
        /// </summary>
        public virtual void OnDisable() { }
    }
}