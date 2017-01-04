using Domo.Misc;
using Domo.Misc.Debug;
using Nancy.Hosting.Self;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domo.API.Web
{
    public class WebAPI : ApiBase
    {
        private const string defaultHostname = "http://localhost:80/api/";
        private NancyHost host;

        public override void Init()
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

        public override void OnShutdown()
        {
            Log.Info("Shutting down web api");

            if (host != null)
                host.Stop();
            else
                Log.Warning("Can't shut down web api, it isn't initialized");
        }

        private void ExceptionHandler(Exception ex)
        {
            Log.Error("Received exception from web api:");
            Log.Exception(ex);
        }
    }
}