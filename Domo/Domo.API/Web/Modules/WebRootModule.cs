using Nancy;

namespace Domo.API.Web.Modules
{
    public class WebRootModule : NancyModule
    {
        public WebRootModule() : base("/")
        {
        }
    }
}