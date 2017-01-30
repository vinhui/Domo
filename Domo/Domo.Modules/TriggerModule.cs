using IronPython.Runtime.Types;

namespace Domo.Modules
{
    public delegate void TriggerEvent();

    public abstract class SensorTriggerModule : TriggerModule
    {
        public void init(PythonType sensorModule)
        {
            base.init(sensorModule, null);
        }
    }

    public abstract class ControllerTriggerModule : TriggerModule
    {
        public void init(PythonType controllerModule)
        {
            base.init(null, controllerModule);
        }
    }

    /// <summary>
    /// Base class for things that need to happen
    /// </summary>
    public abstract class TriggerModule : ModuleBase
    {
        /// <summary>
        /// Access to the sensor
        /// </summary>
        public SensorModule sensor { get; private set; }

        /// <summary>
        /// Access the controller
        /// </summary>
        public ControllerModule controller { get; private set; }

        public event TriggerEvent onTrigger;

        public virtual void init(PythonType sensorModule, PythonType controllerModule)
        {
            if (sensorModule != null)
                sensor = GetModuleReference<SensorModule>(sensorModule);
            if (controllerModule != null)
                controller = GetModuleReference<ControllerModule>(controllerModule);
        }

        public TriggerModule() { }

        /// <summary>
        /// Gets called when it triggers
        /// </summary>
        public virtual void OnTrigger()
        {
            onTrigger?.Invoke();
        }
    }
}