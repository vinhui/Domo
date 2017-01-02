using Nancy;
using System;

namespace Domo.API
{
    public class WebAPI : NancyModule, IApiBase
    {
        public WebAPI()
        {
        }

        public void Init()
        {
        }

        public void OnShutdown()
        {
        }

        public void RegisterListener(string key, Action listener)
        {
        }

        public void UnregisterListener(string key, Action listener)
        {
        }
    }
}