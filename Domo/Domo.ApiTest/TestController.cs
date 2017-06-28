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
            return ApiTestSelfHost.packageActions.Select(x=>x.Key.package.manifest);
        }

        public IEnumerable<string> GetFunctionsFromModule(string module)
        {
            foreach (KeyValuePair<ModuleFactory.ModuleListItem, List<string>> item in ApiTestSelfHost.packageActions)
            {
                if(item.Key.package.manifest.name == module)
                {
                    return item.Value;
                }
            }
            return null;
        }

        [HttpPost]
        public dynamic InvokeFuncFromModule(string module, string func, [FromBody]params object[] parameters)
        {
            
            try
            {
                foreach (KeyValuePair<ModuleFactory.ModuleListItem, List<string>> item in ApiTestSelfHost.packageActions)
                {
                    if (item.Key.package.manifest.name != module)
                        continue;
                    if (!item.Value.Contains(func))
                        continue;

                    dynamic returnVal = item.Key.package.engine.engine.Operations.InvokeMember(item.Key.instance, func, parameters ?? new object[0]);
                    var response = new HttpResponseMessage(HttpStatusCode.Accepted);
                    response.Content = new ObjectContent(returnVal.GetType(), returnVal, new JsonMediaTypeFormatter());
                    return returnVal; 
                }
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
