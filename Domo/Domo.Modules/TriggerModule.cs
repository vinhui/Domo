namespace Domo.Modules
{
    public delegate void TriggerEvent();

    /// <summary>
    /// Base class for things that need to happen based off of sensors
    /// </summary>
    /// <typeparam name="T">Sensor that it is depending on</typeparam>
    /// <typeparam name="U">Controller that it is depending on</typeparam>
    public abstract class TriggerModule<T, U> : TriggerModule
        where T : ISensorModule
        where U : IControllerModule
    {
        /// <summary>
        /// Access to the sensor
        /// </summary>
        [AutoFillGeneric]
        public readonly T sensor;

        /// <summary>
        /// Access the controller
        /// </summary>
        [AutoFillGeneric]
        public readonly U controller;
    }

    /// <summary>
    /// Base class for things that need to happen based off of sensors
    /// </summary>
    /// <typeparam name="T">Sensor that it is depending on</typeparam>
    public abstract class SensorTriggerModule<T> : TriggerModule
        where T : ISensorModule
    {
        /// <summary>
        /// Access to the sensor
        /// </summary>
        [AutoFillGeneric]
        public readonly T sensor;
    }

    /// <summary>
    /// Base class for things that need to happen
    /// </summary>
    /// <typeparam name="T">Controller that it is depending on</typeparam>
    public abstract class ControllerTriggerModule<T> : TriggerModule
        where T : IControllerModule
    {
        /// <summary>
        /// Access to the controller
        /// </summary>
        [AutoFillGeneric]
        public readonly T controller;
    }

    /// <summary>
    /// Base class for things that need to happen
    /// </summary>
    public abstract class TriggerModule : ModuleBase
    {
        public event TriggerEvent onTrigger;

        /// <summary>
        /// Gets called when it triggers
        /// </summary>
        public virtual void OnTrigger()
        {
            onTrigger?.Invoke();
        }
    }
}