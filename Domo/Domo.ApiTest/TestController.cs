using Domo.Misc.Debug;
using Domo.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Domo.Modules;

namespace Domo.ApiTest
{
    public class TestController : ApiController
    {
        public IEnumerable<PackageManifest> GetModules()
        {
            return ApiTestSelfHost.exposedMembers.Select(x=>x.Key.package.manifest);
        }

        public ApiTestSelfHost.ExposedMembers GetFunctionsFromModule(string module)
        {
            foreach (KeyValuePair<ModuleFactory.ModuleListItem, ApiTestSelfHost.ExposedMembers> item in ApiTestSelfHost.exposedMembers)
            {
                if(item.Key.package.manifest.name == module)
                {
                    return item.Value;
                }
            }
            return null;
        }

        public dynamic GetProperty(string module, string property)
        {
            try
            {
                foreach (KeyValuePair<ModuleFactory.ModuleListItem, ApiTestSelfHost.ExposedMembers> item in ApiTestSelfHost.exposedMembers)
                {
                    if (item.Key.package.manifest.name != module)
                        continue;
                    if (!item.Value.properties.ContainsKey(property))
                        continue;

                    dynamic returnVal;
                    if (item.Key.package.engine.engine.Operations.TryGetMember(item.Key.instance, property,
                                                                               out returnVal))
                    {
                        return returnVal;
                    }
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(ex.ToString());
                return response;
            }
        }
        
        [HttpPost]
        public void SetProperty(string module, string property, [FromBody]object value)
        {
            try
            {
                foreach (KeyValuePair<ModuleFactory.ModuleListItem, ApiTestSelfHost.ExposedMembers> item in ApiTestSelfHost.exposedMembers)
                {
                    if (item.Key.package.manifest.name != module)
                        continue;
                    if (!item.Value.properties.ContainsKey(property))
                        continue;

                    item.Key.package.engine.engine.Operations.SetMember(item.Key.instance, property,
                                                                        value);
                    return;
                }
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public dynamic InvokeFuncFromModule(string module, string func, [FromBody]params object[] parameters)
        {
            try
            {
                foreach (KeyValuePair<ModuleFactory.ModuleListItem, ApiTestSelfHost.ExposedMembers> item in ApiTestSelfHost.exposedMembers)
                {
                    if (item.Key.package.manifest.name != module)
                        continue;
                    if (!item.Value.functions.Contains(func))
                        continue;

                    dynamic returnVal = item.Key.package.engine.engine.Operations.InvokeMember(item.Key.instance, func, parameters ?? new object[0]);
                    var response = new HttpResponseMessage(HttpStatusCode.Accepted);
                    response.Content = new ObjectContent(returnVal.GetType(), returnVal, new JsonMediaTypeFormatter());
                    return returnVal; 
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(ex.ToString());
                return response;
            }
        }
    }
}
