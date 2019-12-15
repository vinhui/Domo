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
        public IDictionary<string, string> arguments = new Dictionary<string, string>();

        /// <summary>
        /// Get an argument
        /// </summary>
        /// <typeparam name="T">Type to cast the argument to</typeparam>
        /// <param name="key">Key of the argument</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>Retuns the value or <paramref name="defaultValue"/> if the arguments don't contain the key</returns>
        public T GetArgument<T>(string key, T defaultValue)
        {
            if (HasArgument(key))
                return GetArgument<T>(key);
            else
                return defaultValue;
        }

        /// <summary>
        /// Get an argument
        /// </summary>
        /// <typeparam name="T">Type to cast the argument to</typeparam>
        /// <param name="key">Key of the argument</param>
        /// <returns>Retuns the value or default(<typeparamref name="T"/>)</returns>
        public T GetArgument<T>(string key)
        {
            if (HasArgument(key))
                return (T)Convert.ChangeType(arguments[key], typeof(T));
            else
                return default(T);
        }

        /// <summary>
        /// Get an argument
        /// </summary>
        /// <param name="key">Key of the argument</param>
        /// <returns>Returns the value or null if the arguments don't contain the key</returns>
        public object GetArgument(string key)
        {
            return GetArgument(key, null);
        }

        /// <summary>
        /// Get an argument
        /// </summary>
        /// <param name="key">Key of the argument</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Returns the value or <paramref name="defaultValue"/> if the arguments don't contain the key</returns>
        public object GetArgument(string key, object defaultValue)
        {
            if (HasArgument(key))
                return arguments[key];
            else
                return defaultValue;
        }

        /// <summary>
        /// Check if the arguments contain a certain key
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <returns></returns>
        public bool HasArgument(string key)
        {
            return arguments.ContainsKey(key);
        }
    }
}