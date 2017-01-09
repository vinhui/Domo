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

        /// <summary>
        /// Create a success response
        /// </summary>
        /// <returns>Returns the created api response</returns>
        public static ApiResponse Success()
        {
            return Success(null);
        }

        /// <summary>
        /// Create a success response
        /// </summary>
        /// <param name="data">Data to add</param>
        /// <returns>Returns the created api response</returns>
        public static ApiResponse Success(Dictionary<string, object> data)
        {
            return Success(ApiCodes.Sucess, data);
        }

        /// <summary>
        /// Create a success response
        /// </summary>
        /// <param name="code">Code to return, see <seealso cref="ApiCodes"/></param>
        /// <param name="data">Data to add</param>
        /// <returns>Returns the created api response</returns>
        public static ApiResponse Success(int code, Dictionary<string, object> data)
        {
            return new ApiResponse()
            {
                success = true,
                code = code,
                data = data
            };
        }

        /// <summary>
        /// Create a failed response
        /// </summary>
        /// <param name="code">Code to return, see <seealso cref="ApiCodes"/></param>
        /// <param name="reason">The reason why it failed</param>
        /// <returns>Returns the created api response</returns>
        public static ApiResponse Failed(int code, string reason)
        {
            return Failed(code, reason, null);
        }

        /// <summary>
        /// Create a failed response
        /// </summary>
        /// <param name="code">Code to return, see <seealso cref="ApiCodes"/></param>
        /// <param name="reason">The reason why it failed</param>
        /// <param name="data">Extra data to add</param>
        /// <returns>Returns the created api response</returns>
        public static ApiResponse Failed(int code, string reason, Dictionary<string, object> data)
        {
            if (data == null)
                data = new Dictionary<string, object>();

            data.Add("reason", reason);

            return new ApiResponse()
            {
                success = false,
                code = code,
                data = data
            };
        }
    }
}