using IronPython.Runtime.Types;

namespace Domo.Modules
{
    /// <summary>
    /// Base class for modules that want to control hardware
    /// </summary>
    public abstract class ControllerModule : HardwareDependentModule
    {
    }
}