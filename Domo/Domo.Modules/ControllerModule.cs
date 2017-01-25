using System;

namespace Domo.Modules
{
    /// <summary>
    /// Base class for modules that want to control hardware
    /// </summary>
    /// <exception cref="NotSupportedException">Gets thrown when the hardware interface is read only</exception>
    /// <typeparam name="T">Type of the hardware interface</typeparam>
    public abstract class ControllerModule<T> : HardwareDependentModule<T>, IControllerModule
        where T : HardwareInterfaceModule
    {
        public ControllerModule()
        {
            if (!hardwareInterface.readWriteMode.HasFlag(ReadWriteMode.Write))
                throw new NotSupportedException("This hardware interface can not be controlled");
        }
    }
}