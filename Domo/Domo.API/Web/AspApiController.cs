using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Owin;
using Domo.Misc.Debug;

namespace Domo.API.Web
{
	public class AspApiController : ApiController
	{
		[HttpGet]
		[Route("api/")]
		public HttpResponseMessage Get()
		{
			return ApiResponse.Failed(ApiCodes.NoKeyProvided, "There was no key provided");
		}

		[HttpGet]
		[Route("api/{key}")]
		public HttpResponseMessage GetApi(string key)
		{
			Log.Debug("Api request for key '{0}'", key);
			return RunRequest(
				key,
				Request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value));
		}

		/// <summary>
		/// Run the request through all the api listeners
		/// </summary>
		/// <param name="key">The key to check the listeners for</param>
		/// <param name="arguments">Data to pass through to the listener</param>
		/// <returns>Returns a proper API response that can be directly returned to the client</returns>
		private ApiResponse RunRequest(string key, IDictionary<string, string> arguments)
		{
			if (!string.IsNullOrEmpty(key))
			{
				IReadOnlyDictionary<string, Func<ApiRequest, ApiResponse>> listeners = ApiManager.listeners;

				if (listeners.ContainsKey(key))
				{
					return listeners[key]
						.Invoke(
							new ApiRequest
							{
								key = key,
								arguments = arguments
							}
						);
				}
				else
				{
					return ApiResponse.Failed(ApiCodes.UnknownKey, "The key provided was not recognized");
				}
			}
			else
			{
				return ApiResponse.Failed(ApiCodes.NoKeyProvided, "There was no key provided");
			}
		}
	}
}