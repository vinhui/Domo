using System;
using System.IO;

namespace Domo.Modules
{
    public abstract class HardwareInterfaceModule : ModuleBase
    {
        /// <summary>
        /// Read/write mode of this hardware interface
        /// </summary>
        public virtual ReadWriteMode readWriteMode { get; protected set; } = ReadWriteMode.Both;

        /// <summary>
        /// Is this interface initialized yet
        /// </summary>
        public virtual bool isInitialized { get; protected set; }

        /// <summary>
        /// Is there data available to read
        /// </summary>
        public virtual bool hasDataAvailable { get; protected set; }

        /// <summary>
        /// Send data to the hardware
        /// </summary>
        /// <param name="data">Data to send</param>
        public virtual void SendData(IRawDataObject data)
        {
            if (!readWriteMode.HasFlag(ReadWriteMode.Write))
                throw new InvalidOperationException("Cannot send data to an hardware interface that isn't writable");

            using (MemoryStream memStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(memStream))
            {
                data.Write(writer);
                SendDataRaw(memStream.ToArray());
            }
        }

        /// <summary>
        /// Send data to the hardware
        /// </summary>
        /// <param name="data">Data to send</param>
        public abstract void SendDataRaw(byte[] data);

        /// <summary>
        /// Read data from the hardware
        /// </summary>
        /// <typeparam name="T">Type of object to write the data in to</typeparam>
        /// <param name="obj">Object to write the data in to</param>
        /// <returns>Returns success</returns>
        public virtual bool ReadData<T>(ref T obj) where T : IRawDataObject
        {
            if (!readWriteMode.HasFlag(ReadWriteMode.Read))
                throw new InvalidOperationException("Cannot read data from an hardware interface that isn't readable");

            byte[] bytes;

            if (ReadDataRaw(out bytes))
            {
                using (MemoryStream memStream = new MemoryStream(bytes))
                using (BinaryReader reader = new BinaryReader(memStream))
                {
                    return obj.Read(reader);
                }
            }

            obj = default(T);
            return false;
        }

        /// <summary>
        /// Read data from the hardware
        /// </summary>
        /// <param name="data">Object to write the data in to</param>
        /// <returns>Returns success</returns>
        public abstract bool ReadDataRaw(out byte[] data);
    }
}