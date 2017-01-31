using Domo.Misc.Debug;
using Domo.Serialization;
using Nancy;
using System;
using System.Collections.Generic;

namespace Domo.API.Web.Modules
{
    public class ApiKeyModule : NancyModule
    {
        public ApiKeyModule() : base("/api")
        {
            // Root response
            Get("/", (parameters) =>
            {
                string responseJson = ApiResponse.Failed(ApiCodes.NoKeyProvided, "There was no key provided").Serialize();
                return Response.AsText(responseJson, Serializer.instance.contentType);
            });

            // Response for when a key is provided
            Get("/{key}", (parameters) =>
            {
                Log.Debug("Api request for key '{0}'", parameters.key);

                string responseJson = RunRequest(parameters.key, Request.Query).Serialize();
                return Response.AsText(responseJson, Serializer.instance.contentType);
            });
        }

        /// <summary>
        /// Run the request through all the api listeners
        /// </summary>
        /// <param name="key">The key to check the listeners for</param>
        /// <param name="arguments">Data to pass through to the listener</param>
        /// <returns>Returns a proper API response that can be directly returned to the client</returns>
        private ApiResponse RunRequest(string key, IReadOnlyDictionary<string, object> arguments)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IReadOnlyDictionary<string, Func<ApiRequest, ApiResponse>> listeners = ApiManager.listeners;

                if (listeners.ContainsKey(key))
                {
                    return listeners[key]
                        .Invoke(
                            new ApiRequest()
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