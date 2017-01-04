using System;

namespace Domo.Serialization
{
    /// <summary>
    /// Base class of serializers
    /// </summary>
    public abstract class Serializer
    {
        private static Serializer _instance;

        public static Serializer instance
        {
            get
            {
                if (_instance == null)
                    _instance = new JsonSerializer();

                return _instance;
            }
        }

        /// <summary>
        /// The web type of the serialized data e.g. "application/json"
        /// </summary>
        public abstract string contentType { get; }

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Returns the object in a text format</returns>
        public abstract string Serialize(object obj);

        /// <summary>
        /// Deserialize a string to <paramref name="T"/>
        /// </summary>
        /// <typeparam name="T">Type to deserialize to</typeparam>
        /// <param name="obj">The object in string format</param>
        /// <returns>Returns the converted object</returns>
        public virtual T Deserialize<T>(string obj)
        {
            return (T)Deserialize(typeof(T), obj);
        }

        /// <summary>
        /// Deserialize a string to <paramref name="type"/>
        /// </summary>
        /// <param name="type">Type to deserialize to</param>
        /// <param name="obj">The object in string format</param>
        /// <returns>Returns the converted object</returns>
        public abstract object Deserialize(Type type, string obj);
    }
}