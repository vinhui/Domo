using Domo.Misc.Debug;
using Domo.Serialization;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domo.API.Web.Modules
{
    public class ApiKeyModule : NancyModule
    {
        public ApiKeyModule() : base("/api")
        {
            Get("/{key}", (parameters) =>
            {
                Log.Debug("Api request for key '{0}'", parameters.key);

                string responseJson = RunRequest(parameters.key, Request.Query).Serialize();
                return Response.AsText(responseJson, Serializer.instance.contentType);
            });
        }

        private ApiResponse RunRequest(string key, IDictionary<string, object> arguments)
        {
            if (!string.IsNullOrEmpty(key))
            {
                IEnumerable<KeyValuePair<string, Func<ApiListenerData, ApiResponse>>> listeners = ApiManager.listeners;

                if (listeners.Any(x => x.Key == key))
                {
                    return listeners
                        .First(x => x.Key == key)
                        .Value
                        .Invoke(
                            new ApiListenerData()
                            {
                                key = key,
                                arguments = arguments
                            }
                        );
                }
                else
                {
                    return new ApiResponse()
                    {
                        code = ApiCodes.UnknownKey,
                        success = false
                    };
                }
            }
            else
            {
                return new ApiResponse()
                {
                    code = ApiCodes.NoKeyProvided,
                    success = false,
                };
            }
        }
    }
}