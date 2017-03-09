using Domo.Misc.Debug;
using Domo.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            foreach (var item in ApiTestSelfHost.packageActions)
            {
                if(item.Key.name == module)
                {
                    return item.Value.Keys;
                }
            }
            return null;
        }

        [HttpPost]
        public HttpResponseMessage InvokeFuncFromModule(string module, string func)
        {
            try
            {
                foreach (KeyValuePair<ModuleFactory.ModuleListItem, Dictionary<string, Action>> item in ApiTestSelfHost.packageActions)
                {
                    if (item.Key.package.manifest.name != module)
                        continue;
                    if (!item.Value.ContainsKey(func))
                        continue;

                    item.Key.package.engine.engine.Operations.InvokeMember(item.Key.instance, func);
                    //item.Value[func].Invoke();
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
