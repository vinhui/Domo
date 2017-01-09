using System.IO;

namespace Domo.Modules
{
    public interface IRawDataObject
    {
        /// <summary>
        /// Read data from the binary reader into the inherited object
        /// </summary>
        /// <param name="reader">Reader to read from</param>
        /// <returns>Should return a success bool</returns>
        bool Read(BinaryReader reader);

        /// <summary>
        /// Write all the data you want saved into the writer
        /// </summary>
        /// <param name="writer">Writer to write to</param>
        void Write(BinaryWriter writer);
    }
}