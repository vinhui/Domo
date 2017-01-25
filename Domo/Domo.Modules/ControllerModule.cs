using System;

namespace Domo.Modules
{
    /// <summary>
    /// Base class for modules that want to control hardware
    /// </summary>
    /// <typeparam name="T">Type of the hardware interface it is depended on</typeparam>
    public abstract class ControllerModule<T> : HardwareDependentModule<T>, IControllerModule
        where T : HardwareInterfaceModule
    {

    }

    /// <summary>
    /// Base class for modules that want to control things
    /// </summary>
    public abstract class ControllerModule : ModuleBase, IControllerModule
    {

    }
}