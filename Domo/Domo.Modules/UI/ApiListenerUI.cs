using Domo.API;
using System;
using System.Collections.Generic;

namespace Domo.Modules.UI
{
    public class ApiListenerUI
    {
        private const string actionGetParam = "action";
        private Dictionary<string, Func<ApiRequest, ApiResponse>> actions;

        public ApiListenerUI()
        {
            ApiManager.RegisterListener("ui", RequestHandler);

            actions = new Dictionary<string, Func<ApiRequest, ApiResponse>>()
            {
                { "getModules", GetModules }
            };
        }

        public ApiResponse RequestHandler(ApiRequest request)
        {
            if (request.arguments.ContainsKey(actionGetParam))
            {
                string act = request.GetArgument<string>(actionGetParam);
                if (actions.ContainsKey(act))
                {
                    return actions[act].Invoke(request);
                }
                else
                {
                    return ApiResponse.Failed(
                            ApiCodes.NotEnoughData,
                            "Action passed is invalid",
                            new Dictionary<string, object>()
                            {
                                { "availableActions", actions.Keys }
                            }
                        );
                }
            }
            else
            {
                return ApiResponse.Failed(
                        ApiCodes.NotEnoughData,
                        "There is no action defined",
                        new Dictionary<string, object>()
                        {
                            { "availableActions", actions.Keys }
                        }
                    );
            }
        }

        public ApiResponse GetModules(ApiRequest request)
        {
            IEnumerable<UIModule> modules = UIModule.GetModules();

            return ApiResponse.Success(
                ApiCodes.Sucess,
                new Dictionary<string, object>()
                {
                    { "modules", modules }
                }
            );
        }
    }
}