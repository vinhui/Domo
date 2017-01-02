using System;

namespace Domo.API
{
    public interface IApiBase
    {
        void Init();

        void RegisterListener(string key, Action<ApiListenerData> listener);

        void UnregisterListener(string key, Action<ApiListenerData> listener);

        void OnShutdown();
    }
}