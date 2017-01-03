using Domo.Misc;
using Domo.Misc.Debug;
using Nancy.Hosting.Self;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domo.API.Web
{
    public class WebAPI : IApiBase
    {
        internal static WebAPI instance;
        private const string defaultHostname = "http://localhost:80/api/";
        private NancyHost host;
        private List<KeyValuePair<string, Func<ApiListenerData, ApiResponse>>> _listeners = new List<KeyValuePair<string, Func<ApiListenerData, ApiResponse>>>();
        public static IEnumerable<KeyValuePair<string, Func<ApiListenerData, ApiResponse>>> listeners { get { return instance._listeners; } }

        public WebAPI()
        {
            if (instance == null)
                instance = this;
            else
                Log.Error("You are trying to create multiple instances of WebAPI");
        }

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

            string[] hostnames = Config.GetValue<ArrayList>("API", "web", "bindurls").ToArray(typeof(string)) as string[];
            Uri[] uris;
            if (hostnames != null && hostnames.Length > 0)
            {
                uris = hostnames.Select(x => new Uri(x)).ToArray();
                Log.Debug("Binding web api to the following urls: {0}", string.Join(", ", hostnames));
            }
            else
            {
                Log.Warning("No 'API.web.bindurls' found in the config, using the default: {0}", defaultHostname);
                uris = new Uri[]
                {
                    new Uri(defaultHostname)
                };
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

        public void RegisterListener(string key, Func<ApiListenerData, ApiResponse> listener)
        {
            _listeners.Add(new KeyValuePair<string, Func<ApiListenerData, ApiResponse>>(key, listener));
        }

        public void UnregisterListener(string key, Func<ApiListenerData, ApiResponse> listener)
        {
            int cnt = _listeners.Count(x => x.Key == key && x.Value == listener);
            Log.Info("Unregistering api listener for key '{0}', found {1} item{2}", key, cnt, (cnt != 1 ? "s" : ""));
            if (cnt > 0)
                _listeners.RemoveAll(x => x.Key == key && x.Value == listener);
        }

        private void ExceptionHandler(Exception ex)
        {
            Log.Error("Received exception from web api:");
            Log.Exception(ex);
        }
    }
}