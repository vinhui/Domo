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
        public IEnumerable<string> GetModules()
        {
            return ApiTestSelfHost.packageActions.Keys;
        }

        public IEnumerable<string> GetFunctionsFromModule(string module)
        {
            if (ApiTestSelfHost.packageActions.ContainsKey(module))
                return ApiTestSelfHost.packageActions[module].Keys;
            return null;
        }

        [HttpPost]
        public HttpResponseMessage InvokeFuncFromModule(string module, string func)
        {
            try
            {
                if (ApiTestSelfHost.packageActions.ContainsKey(module))
                {
                    if (ApiTestSelfHost.packageActions[module].ContainsKey(func))
                    {
                        ApiTestSelfHost.packageActions[module][func].Invoke();
                        return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
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
