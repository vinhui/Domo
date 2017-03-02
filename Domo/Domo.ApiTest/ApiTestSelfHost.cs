﻿using Domo.Packaging;
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
        public static Dictionary<PackageManifest, Dictionary<string, Action>> packageActions = new Dictionary<PackageManifest, Dictionary<string, Action>>();

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
                        try
                        {
                            var classDict = variable.Value.__dict__ as IDictionary<dynamic, dynamic>;
                            foreach (var item in classDict)
                            {
                                try
                                {
                                    var action = item.Value.__func__.ApiActionInvoke;
                                    if (!packageActions.ContainsKey(package.manifest))
                                    {
                                        packageActions.Add(package.manifest, new Dictionary<string, Action>());
                                    }
                                    packageActions[package.manifest].Add(item.Key, action);
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            var action = variable.Value.ApiActionInvoke;
                            if (!packageActions.ContainsKey(package.manifest))
                            {
                                packageActions.Add(package.manifest, new Dictionary<string, Action>());
                            }
                            packageActions[package.manifest].Add(variable.Key, action);
                        }
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
