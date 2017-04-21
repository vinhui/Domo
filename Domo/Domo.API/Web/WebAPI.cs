using Domo.Misc;
using Domo.Misc.Debug;
using System;
using System.Collections;
using System.Net;
using Microsoft.Owin.Hosting;

#if __MonoCS__
using System.Runtime.InteropServices;
#endif

namespace Domo.API.Web
{
	public class WebAPI : ApiBase
	{
		private const string defaultHostname = "http://+:80/api/";

#if __MonoCS__
        [DllImport ("libc")]
        public static extern uint getuid ();
#endif

		private IDisposable[] webApps;

		public override void Init()
		{
			Log.Debug("Starting web api");

#if __MonoCS__
            if (getuid() == 0)
            {
                Log.Info("Running as root, everything is fine");
            }
            else
            {
                Log.Error("You need to run as root to start the web api!");
                return;
            }
#endif

			string[] hostnames = Config.GetValue<ArrayList>("API", "web", "bindurls").ToArray(typeof(string)) as string[];

			if (hostnames == null || hostnames.Length == 0)
			{
				Log.Warning("No 'API.web.bindurls' found in the config, using the default: {0}", defaultHostname);
				hostnames = new[]
				{
					defaultHostname
				};
			}

			Log.Debug("Binding web api to the following urls: {0}", string.Join(", ", hostnames));
			webApps = new IDisposable[hostnames.Length];

			for (int i = 0; i < hostnames.Length; i++)
			{
				try
				{
					webApps[i] = WebApp.Start<AspApiConfiguration>(hostnames[i]);
				}
				catch (HttpListenerException ex)
				{
					Log.Error("Failed to bind api listener to '{0}'", hostnames[i]);
					Log.Exception(ex);
				}
			}

			Log.Info("Web api has started");
		}

		public override void OnShutdown()
		{
			Log.Info("Shutting down web api");

			foreach (IDisposable webApp in webApps)
			{
				webApp?.Dispose();
			}
		}

		private void ExceptionHandler(Exception ex)
		{
			Log.Error("Received exception from web api:");
			Log.Exception(ex);
		}
	}
}