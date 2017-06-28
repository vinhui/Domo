using Domo.Misc.Debug;
using Domo.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using IronPython.Runtime.Operations;
using static Domo.Modules.ModuleFactory;

namespace Domo.ApiTest
{
    public class ApiTestSelfHost
    {
        private static HttpSelfHostServer server;

        public static Dictionary<ModuleListItem, List<string>> packageActions =
            new Dictionary<ModuleListItem, List<string>>();

        public ApiTestSelfHost(IEnumerable<ModuleListItem> modules)
        {
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration("http://localhost:80");

            foreach (ModuleListItem module in modules)
            {
                try
                {
                    dynamic c = null;
                    PythonOps.TryGetBoundAttr(module.instance, "__class__", out c);
                    //                    IDictionary<dynamic, dynamic> classDict = module.dynamicInstance.__dict__ as IDictionary<dynamic, dynamic>;
                    IDictionary<dynamic, dynamic> classDict = c.__dict__ as IDictionary<dynamic, dynamic>;
                    if (classDict == null)
                        continue;

                    foreach (KeyValuePair<dynamic, dynamic> item in classDict)
                    {
                        try
                        {
                            IDictionary<dynamic, dynamic> keyValuePairs =
                                item.Value.__dict__ as IDictionary<dynamic, dynamic>;
                            if (keyValuePairs == null)
                                continue;

                            foreach (KeyValuePair<dynamic, dynamic> variable in
                                keyValuePairs)
                            {
                                if (variable.Key != "ApiActionInvoke")
                                    continue;

                                if (!packageActions.ContainsKey(module))
                                {
                                    packageActions.Add(module, new List<string>());
                                }
                                packageActions[module].Add(item.Key);
                            }
                            //                            dynamic action = item.Value.__func__.ApiActionInvoke;
                            //                            if (!packageActions.ContainsKey(module.package.manifest))
                            //                            {
                            //                                packageActions.Add(module.package.manifest, new Dictionary<string, Action>());
                            //                            }
                            //                            packageActions[module.package.manifest].Add(item.Key, action);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                //finally
                //{
                //    var action = variable.Value.ApiActionInvoke;
                //    if (!packageActions.ContainsKey(package.manifest))
                //    {
                //        packageActions.Add(package.manifest, new Dictionary<string, Action>());
                //    }
                //    packageActions[package.manifest].Add(variable.Key, action);
                //}
            }

            //foreach (var package in packages)
            //{
            //    var variables = package.engine.GetVariables();
            //    foreach (var variable in variables)
            //    {
            //        try
            //        {
            //            try
            //            {
            //                var classDict = variable.Value.__dict__ as IDictionary<dynamic, dynamic>;
            //                foreach (var item in classDict)
            //                {
            //                    try
            //                    {
            //                        var action = item.Value.__func__.ApiActionInvoke;
            //                        if (!packageActions.ContainsKey(package.manifest))
            //                        {
            //                            packageActions.Add(package.manifest, new Dictionary<string, Action>());
            //                        }
            //                        packageActions[package.manifest].Add(item.Key, action);
            //                    }
            //                    catch
            //                    {
            //                        continue;
            //                    }
            //                }
            //            }
            //            catch
            //            {
            //            }
            //            finally
            //            {
            //                var action = variable.Value.ApiActionInvoke;
            //                if (!packageActions.ContainsKey(package.manifest))
            //                {
            //                    packageActions.Add(package.manifest, new Dictionary<string, Action>());
            //                }
            //                packageActions[package.manifest].Add(variable.Key, action);
            //            }
            //        }
            //        catch
            //        {
            //            continue;
            //        }
            //    }
            //}

            config.Formatters.Clear();
            config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());

            config.Routes.MapHttpRoute(
                                       "InvokeFunc", "api/modules/{module}/{func}",
                                       defaults: new
                                                 {
                                                     controller = "Test",
                                                     func = RouteParameter.Optional,
                                                     module = RouteParameter.Optional
                                                 });

            server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
        }

        public static void Unload()
        {
            server?.Dispose();
        }
    }
}