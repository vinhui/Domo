namespace Domo.Modules
{
    public abstract class SensorModule<T> : HardwareDependentModule<T> where T : HardwareInterfaceModule, ISensorModule, new()
    {
    }
}