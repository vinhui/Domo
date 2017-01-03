using System.Collections.Generic;

namespace Domo.API
{
    public class ApiListenerData
    {
        public string key;
        public IDictionary<string, object> arguments = new Dictionary<string, object>();
    }
}