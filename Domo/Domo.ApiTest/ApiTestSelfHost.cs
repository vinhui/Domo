using Domo.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;

namespace Domo.ApiTest
{
    public class ApiTestSelfHost
    {
        static HttpSelfHostServer server;
        public static Dictionary<string, Dictionary<string, Action>> packageActions = new Dictionary<string, Dictionary<string, Action>>();

        public ApiTestSelfHost(List<Package> packages)
        {
            var config = new HttpSelfHostConfiguration("http://localhost:80");

            foreach (var package in packages)
            {
                var variables = package.engine.GetVariables();
                foreach (var variable in variables)
                {
                    try
                    {
                        var action = variable.Value.ApiActionInvoke;
                        if(!packageActions.ContainsKey(package.manifest.name))
                        {
                            packageActions.Add(package.manifest.name, new Dictionary<string, Action>());
                        }
                        packageActions[package.manifest.name].Add(variable.Key, action);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            config.Formatters.Clear();
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());

            config.Routes.MapHttpRoute(
                "InvokeFunc", "api/modules/{module}/{func}",
                defaults: new { controller = "Test", func = RouteParameter.Optional, module = RouteParameter.Optional });

            server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
        }

        public static void Unload()
        {
            if (server != null)
                server.Dispose();
        }
    }
}
