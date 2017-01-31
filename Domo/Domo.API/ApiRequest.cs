using System;
using System.Collections.Generic;

namespace Domo.API
{
    /// <summary>
    /// An object that gets passed to API listeners when there is a request for the registered key
    /// </summary>
    public class ApiRequest
    {
        /// <summary>
        /// The key the request is for
        /// </summary>
        public string key;

        /// <summary>
        /// Arguments from the request
        /// </summary>
        public IReadOnlyDictionary<string, object> arguments = new Dictionary<string, object>();

        /// <summary>
        /// Get an argument
        /// </summary>
        /// <typeparam name="T">Type to cast the argument to</typeparam>
        /// <param name="key">Key of the argument</param>
        /// <returns>Retuns the value or default(<typeparamref name="T"/>)</returns>
        public T GetArgument<T>(string key)
        {
            return (T)Convert.ChangeType(arguments[key], typeof(T));
        }
    }
}