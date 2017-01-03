using System;
using System.Web.Script.Serialization;

namespace Domo.Serialization
{
    public class JsonSerializer : Serializer
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        public override string contentType
        {
            get { return "application/json"; }
        }

        public override string Serialize(object obj)
        {
            return serializer.Serialize(obj);
        }

        public override T Deserialize<T>(string obj)
        {
            return serializer.Deserialize<T>(obj);
        }

        public override object Deserialize(Type type, string obj)
        {
            return serializer.Deserialize(obj, type);
        }
    }
}