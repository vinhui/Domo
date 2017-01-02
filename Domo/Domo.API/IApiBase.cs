using System;

namespace Domo.API
{
    internal interface IApiBase
    {
        void Init();

        void RegisterListener(string key, Action listener);

        void UnregisterListener(string key, Action listener);

        void OnShutdown();
    }
}