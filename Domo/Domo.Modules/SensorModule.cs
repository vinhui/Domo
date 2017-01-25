using System;

namespace Domo.Modules
{
    public abstract class SensorModule<T> : HardwareDependentModule<T>, ISensorModule 
        where T : HardwareInterfaceModule
    {
        public SensorModule()
        {
            if (!hardwareInterface.readWriteMode.HasFlag(ReadWriteMode.Read))
                throw new NotSupportedException("This hardware interface does not support reading from");
        }
    }
}