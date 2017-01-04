using Domo.Serialization;
using System.Collections.Generic;

namespace Domo.API
{
    /// <summary>
    /// Object that gets serialized and returned to the client
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// See <see cref="ApiCodes"/> for the meaning of the codes
        /// </summary>
        public int code;
        /// <summary>
        /// If the request was successfully processed
        /// </summary>
        public bool success;
        /// <summary>
        /// The data that gets returned by the corresponding API listener
        /// </summary>
        public Dictionary<string, object> data = new Dictionary<string, object>();

        /// <summary>
        /// Serialize this object to a format that can be processed by other programs
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return Serializer.instance.Serialize(this);
        }
    }
}