using System;
using Newtonsoft.Json;

namespace Domo.Serialization
{
    public class JsonSerializer : Serializer
    {
        public override string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public override T Deserialize<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public override object Deserialize(Type type, string obj)
        {
            return JsonConvert.DeserializeObject(obj, type);
        }
    }
}