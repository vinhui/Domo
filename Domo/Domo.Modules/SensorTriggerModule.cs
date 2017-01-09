namespace Domo.Modules
{
    public abstract class SensorTriggerModule<T, U> : ModuleBase where T : SensorModule<U> where U : HardwareInterfaceModule
    {
        public delegate void SensorTriggerEvent(SensorModule<U> sensor);

        /// <summary>
        /// Gets called when it triggers
        /// </summary>
        public event SensorTriggerEvent onTrigger;

        /// <summary>
        /// Access to the sensor
        /// </summary>
        public T sensor { get; private set; }

        /// <summary>
        /// Is it allowed to trigger
        /// </summary>
        /// <returns>If it's allowed to trigger</returns>
        public abstract bool AllowedToTrigger();

        /// <summary>
        /// Gets called when it triggers
        /// </summary>
        public virtual void OnTrigger()
        {
            onTrigger(sensor);
        }
    }
}