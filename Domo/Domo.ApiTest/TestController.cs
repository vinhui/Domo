using Domo.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Domo.ApiTest
{
    public class TestController : ApiController
    {
        public IEnumerable<PackageManifest> GetModules()
        {
            return ApiTestSelfHost.packageActions.Keys;
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
                foreach (var item in ApiTestSelfHost.packageActions)
                {
                    if (item.Key.name == module)
                    {
                        if (item.Value.ContainsKey(func))
                        {
                            item.Value[func].Invoke();
                            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
                        }
                    }
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }
            catch
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
