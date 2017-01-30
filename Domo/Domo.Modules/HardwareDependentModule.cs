using IronPython.Runtime.Types;
using System;

namespace Domo.Modules
{
    /// <summary>
    /// Base class for any modules that depend on hardware
    /// </summary>
    /// <typeparam name="T">Hardware interface it depends on</typeparam>
    public abstract class HardwareDependentModule : ModuleBase
    {
        /// <summary>
        /// Access to the hardware interface
        /// </summary>
        public HardwareInterfaceModule hardwareInterface { get; private set; }

        public virtual void init(PythonType dependency)
        {
            if (dependency != null)
                hardwareInterface = GetModuleReference<HardwareInterfaceModule>(dependency);
        }

        /// <summary>
        /// Send data to the hardware
        /// </summary>
        /// <exception cref="NotSupportedException">Gets thrown when the interface not be written to</exception>
        /// <exception cref="NotInitializedException">Gets thrown when the hardware interface is not yet initialized</exception>
        /// <param name="data">Data to send</param>
        public virtual void SendData(IRawDataObject data)
        {
            if (!hardwareInterface.readWriteMode.HasFlag(ReadWriteMode.Write))
                throw new NotSupportedException("This hardware interface cannot be written to");

            if (hardwareInterface.isInitialized)
                hardwareInterface.SendData(data);
            else
                throw new NotInitializedException("Data cannot be send before the hardware interface is initialized");
        }

        /// <summary>
        /// Send data to the hardware
        /// </summary>
        /// <exception cref="NotSupportedException">Gets thrown when the interface not be written to</exception>
        /// <exception cref="NotInitializedException">Gets thrown when the hardware interface is not yet initialized</exception>
        /// <param name="data">Data to send</param>
        public virtual void SendDataRaw(byte[] data)
        {
            if (!hardwareInterface.readWriteMode.HasFlag(ReadWriteMode.Write))
                throw new NotSupportedException("This hardware interface cannot be written to");

            if (hardwareInterface.isInitialized)
                hardwareInterface.SendDataRaw(data);
            else
                throw new NotInitializedException("Data cannot be send before the hardware interface is initialized");
        }

        /// <summary>
        /// Read data from the hardware
        /// </summary>
        /// <typeparam name="U">Type of object to read data in to</typeparam>
        /// <param name="data">Object to read data in to</param>
        /// <exception cref="NotSupportedException">Gets thrown when the hardware interface cannot be read from</exception>
        /// <returns>Returns success</returns>
        public virtual bool ReadData<U>(ref U data) where U : IRawDataObject
        {
            if (!hardwareInterface.readWriteMode.HasFlag(ReadWriteMode.Read))
                throw new NotSupportedException("This hardware interface cannot be read from");

            if (hardwareInterface.hasDataAvailable)
                return hardwareInterface.ReadData(ref data);

            data = default(U);
            return false;
        }

        /// <summary>
        /// Read data from the hardware
        /// </summary>
        /// <param name="data">Array to write data to</param>
        /// <exception cref="NotSupportedException">Gets thrown when the hardware interface cannot be read from</exception>
        /// <returns>Returns success</returns>
        public virtual bool ReadDataRaw(out byte[] data)
        {
            if (!hardwareInterface.readWriteMode.HasFlag(ReadWriteMode.Read))
                throw new NotSupportedException("This hardware interface cannot be read from");

            if (hardwareInterface.hasDataAvailable)
                return hardwareInterface.ReadDataRaw(out data);

            data = null;
            return false;
        }
    }
}