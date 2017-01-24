namespace Domo.Modules
{
    /// <summary>
    /// Base class for things that need to happen based off of sensors
    /// </summary>
    /// <typeparam name="T">Sensor that should be auto-filled</typeparam>
    /// <typeparam name="U">Controller that should be auto-filled</typeparam>
    public abstract class SensorTriggerModule<T, U> : SensorTriggerModule<T>
        where T : ISensorModule
        where U : IControllerModule
    {
        /// <summary>
        /// Access the controller
        /// </summary>
        [AutoFillGeneric]
        public readonly U controller;
    }

    /// <summary>
    /// Base class for things that need to happen based off of sensors
    /// </summary>
    /// <typeparam name="T">Sensor that should be auto-filled</typeparam>
    public abstract class SensorTriggerModule<T> : SensorTriggerModule where T : ISensorModule
    {
        public delegate void SensorTriggerEvent(T sensor);

        /// <summary>
        /// Gets called when it triggers
        /// </summary>
        public event SensorTriggerEvent onTrigger;

        /// <summary>
        /// Access to the sensor
        /// </summary>
        [AutoFillGeneric]
        public readonly T sensor;

        /// <summary>
        /// Gets called when it triggers
        /// </summary>
        public override void OnTrigger()
        {
            onTrigger(sensor);
        }
    }

    /// <summary>
    /// Base class for things that need to happen based off of sensors
    /// </summary>
    public abstract class SensorTriggerModule : ModuleBase
    {
        /// <summary>
        /// Is it allowed to trigger
        /// </summary>
        /// <returns>If it's allowed to trigger</returns>
        public abstract bool AllowedToTrigger();

        /// <summary>
        /// Gets called when it triggers
        /// </summary>
        public abstract void OnTrigger();
    }
}