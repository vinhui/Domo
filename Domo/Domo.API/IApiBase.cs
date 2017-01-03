using System;

namespace Domo.API
{
    public interface IApiBase
    {
        void Init();

        void RegisterListener(string key, Func<ApiListenerData, ApiResponse> listener);

        void UnregisterListener(string key, Func<ApiListenerData, ApiResponse> listener);

        void OnShutdown();
    }
}