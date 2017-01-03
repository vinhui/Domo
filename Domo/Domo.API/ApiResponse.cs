using Domo.Serialization;
using System.Collections.Generic;

namespace Domo.API
{
    public class ApiResponse
    {
        public int code;
        public bool success;
        public Dictionary<string, object> data = new Dictionary<string, object>();

        public string Serialize()
        {
            return Serializer.instance.Serialize(this);
        }
    }
}