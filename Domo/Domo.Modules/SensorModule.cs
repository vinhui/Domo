namespace Domo.Modules
{
    public abstract class SensorModule<T> : HardwareDependentModule<T>, ISensorModule 
        where T : HardwareInterfaceModule
    {
    }
}