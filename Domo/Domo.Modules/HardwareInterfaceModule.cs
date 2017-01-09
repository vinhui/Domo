using System.IO;

namespace Domo.Modules
{
    public abstract class HardwareInterfaceModule : ModuleBase
    {
        /// <summary>
        /// Is this hardware interface read only
        /// </summary>
        public abstract bool isReadOnly { get; protected set; }

        /// <summary>
        /// Is this interface initialized yet
        /// </summary>
        public abstract bool isInitialized { get; protected set; }

        /// <summary>
        /// Is there data available to read
        /// </summary>
        public abstract bool hasDataAvailable { get; protected set; }

        /// <summary>
        /// Send data to the hardware
        /// </summary>
        /// <param name="data">Data to send</param>
        public virtual void SendData(IRawDataObject data)
        {
            using (MemoryStream memStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(memStream))
            {
                data.Write(writer);
                SendData(memStream.ToArray());
            }
        }

        /// <summary>
        /// Send data to the hardware
        /// </summary>
        /// <param name="data">Data to send</param>
        public abstract void SendData(byte[] data);

        /// <summary>
        /// Read data from the hardware
        /// </summary>
        /// <typeparam name="T">Type of object to write the data in to</typeparam>
        /// <param name="obj">Object to write the data in to</param>
        /// <returns>Returns success</returns>
        public virtual bool ReadData<T>(out T obj) where T : IRawDataObject, new()
        {
            if (isReadOnly)
            {
                obj = default(T);
                return false;
            }

            byte[] bytes;

            if (ReadData(out bytes))
            {
                using (MemoryStream memStream = new MemoryStream(bytes))
                using (BinaryReader reader = new BinaryReader(memStream))
                {
                    obj = new T();
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
        public abstract bool ReadData(out byte[] data);
    }
}