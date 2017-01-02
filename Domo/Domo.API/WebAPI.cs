using Domo.Misc;
using Domo.Misc.Debug;
using Nancy.Hosting.Self;
using System;

namespace Domo.API
{
    public class WebAPI : IApiBase
    {
        public NancyHost host;

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
        }

        public void UnregisterListener(string key, Action<ApiListenerData> listener)
        {
        }

        private void ExceptionHandler(Exception ex)
        {
            Log.Error("Received exception from web api:");
            Log.Exception(ex);
        }
    }
}