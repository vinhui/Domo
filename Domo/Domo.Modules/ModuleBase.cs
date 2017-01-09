namespace Domo.Modules
{
    public abstract class ModuleBase
    {
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