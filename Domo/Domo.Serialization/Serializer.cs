using System;

namespace Domo.Serialization
{
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

        public abstract string contentType { get; }

        public abstract string Serialize(object obj);

        public virtual T Deserialize<T>(string obj)
        {
            return (T)Deserialize(typeof(T), obj);
        }

        public abstract object Deserialize(Type type, string obj);
    }
}