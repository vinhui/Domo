using System;

namespace Domo.WebAPI
{
    internal interface IApiBase
    {
        void Init();

        void RegisterListener(string key, Action listener);

        void UnregisterListener(string key, Action listener);

        void OnShutdown();
    }
}