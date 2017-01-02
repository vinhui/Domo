using Domo.Misc;
using Domo.Misc.Debug;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domo.API
{
    public class WebAPI : IApiBase
    {
        private NancyHost host;
        private List<KeyValuePair<string, Action<ApiListenerData>>> listeners = new List<KeyValuePair<string, Action<ApiListenerData>>>();

        public void Init()
        {
            Log.Debug("Starting web api");

            HostConfiguration hostConfig = new HostConfiguration()
            {
                UnhandledExceptionCallback = ExceptionHandler,
                UrlReservations = new UrlReservations()
                {
                    CreateAutomatically = true
                }
            };

            string[] hostnames = Config.GetValue<string[]>("API", "web", "hostnames");
            int port = Config.GetValue<int>("API", "web", "port");

            Uri[] uris = new Uri[hostnames.Length];

            for (int i = 0; i < hostnames.Length; i++)
            {
                uris[i] = new Uri(string.Format("http://{0}:{1}", hostnames[i], port));
            }

            host = new NancyHost(hostConfig, uris);
            host.Start();
            Log.Info("Web api has started");
        }

        public void OnShutdown()
        {
            Log.Info("Shutting down web api");

            if (host != null)
                host.Stop();
            else
                Log.Warning("Can't shut down web api, it isn't initialized");
        }

        public void RegisterListener(string key, Action<ApiListenerData> listener)
        {
            listeners.Add(new KeyValuePair<string, Action<API.ApiListenerData>>(key, listener));
        }

        public void UnregisterListener(string key, Action<ApiListenerData> listener)
        {
            int cnt = listeners.Count(x => x.Key == key && x.Value == listener);
            Log.Info("Unregistering api listener for key '{0}', found {1} item{2}", key, cnt, (cnt != 1 ? "s" : ""));
            if (cnt > 0)
                listeners.RemoveAll(x => x.Key == key && x.Value == listener);
        }

        private void ExceptionHandler(Exception ex)
        {
            Log.Error("Received exception from web api:");
            Log.Exception(ex);
        }
    }
}