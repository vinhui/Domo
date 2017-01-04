using Domo.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy;
using Nancy.Testing;

namespace Domo.Testing
{
    [TestClass]
    public class ApiTest
    {
        [TestMethod]
        public void WebServer()
        {
            TestUtils.SetEntryAssembly();

            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper);

            ApiManager.Init();

            var result = browser.Get("/api/", with =>
            {
                with.HttpRequest();
            });

            result.Wait();

            ApiManager.OnShutdown();
            Assert.AreEqual(HttpStatusCode.OK, result.Result.StatusCode);
        }

        [TestMethod]
        public void WebApi()
        {
            TestUtils.SetEntryAssembly();

            var bootstrapper = new DefaultNancyBootstrapper();
            var browser = new Browser(bootstrapper, defaults: to => to.Accept("application/json"));

            ApiManager.Init();
            ApiManager.RegisterListener("testing", TestListener);

            var result = browser.Get("/api/testing", with =>
            {
                with.HttpRequest();
                with.Query("Foo", "Bar");
            });

            result.Wait();

            ApiManager.OnShutdown();
            Assert.IsTrue(result.Result.Body.AsString().Contains("Foo"));
            Assert.IsTrue(result.Result.Body.AsString().Contains("Bar"));
        }

        private static ApiResponse TestListener(ApiRequest data)
        {
            return new ApiResponse()
            {
                code = 0,
                success = true,
                data = new System.Collections.Generic.Dictionary<string, object>(data.arguments)
            };
        }
    }
}