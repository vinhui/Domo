using System.Web.Http;
using Owin;

namespace Domo.API.Web
{
	public class AspApiConfiguration
	{
		public void Configuration(IAppBuilder app)
		{
			HttpConfiguration webApiConfiguration = ConfigureWebApi();

			// Use the extension method provided by the WebApi.Owin library:
			app.UseWebApi(webApiConfiguration);
		}

		private HttpConfiguration ConfigureWebApi()
		{
			HttpConfiguration config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();

			return config;
		}
	}
}