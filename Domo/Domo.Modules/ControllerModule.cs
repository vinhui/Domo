using System;

namespace Domo.Modules
{
    /// <summary>
    /// Base class for modules that want to control hardware
    /// </summary>
    /// <exception cref="NotSupportedException">Gets thrown when the hardware interface is read only</exception>
    /// <typeparam name="T">Type of the hardware interface</typeparam>
    public abstract class ControllerModule<T> : HardwareDependentModule<T> where T : HardwareInterfaceModule
    {
        public ControllerModule()
        {
            if (hardwareInterface.isReadOnly)
                throw new NotSupportedException("This hardware interface is read only");
        }
    }
}