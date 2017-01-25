using System;

namespace Domo.Modules
{
    /// <summary>
    /// Base class for any modules that read sensor information
    /// </summary>
    /// <typeparam name="T">Type of hardware interface it is depended on</typeparam>
    public abstract class SensorModule<T> : HardwareDependentModule<T>, ISensorModule 
        where T : HardwareInterfaceModule
    {

    }

    /// <summary>
    /// Base class for any modules that read sensor information
    /// </summary>
    public abstract class SensorModule : ISensorModule
    {

    }
}