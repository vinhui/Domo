using Domo.Misc.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Domo.API
{
    public static class ApiManager
    {
        private static List<ApiBase> _apis;
        public static IEnumerable<ApiBase> apis { get { return _apis; } }

        private static List<KeyValuePair<string, Func<ApiListenerData, ApiResponse>>> _listeners;
        public static IEnumerable<KeyValuePair<string, Func<ApiListenerData, ApiResponse>>> listeners { get { return _listeners; } }

        public static void Init()
        {
            _apis = new List<ApiBase>();
            _listeners = new List<KeyValuePair<string, Func<ApiListenerData, ApiResponse>>>();

            GetApis();

            foreach (var item in apis)
                item.Init();
        }

        private static void GetApis()
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
            int cnt = 0;

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(ApiBase)))
                    {
                        if (!type.IsAbstract)
                        {
                            ApiBase instance = Activator.CreateInstance(type) as ApiBase;
                            _apis.Add(instance);
                            Log.Debug("Found api '{0}' in assembly '{1}'", type.Name, assembly.FullName);
                            cnt++;
                        }
                    }
                }
            }

            Log.Info("Found {0} apis", cnt);
        }

        public static void RegisterListener(string key, Func<ApiListenerData, ApiResponse> listener)
        {
            Log.Info("Registering api listener for key '{0}'", key);
            _listeners.Add(new KeyValuePair<string, Func<ApiListenerData, ApiResponse>>(key, listener));

            foreach (var item in apis)
            {
                item.OnListenerRegistered(key);
                item.OnListenerRegistered(key, listener);
            }
        }

        public static void UnregisterListener(string key, Func<ApiListenerData, ApiResponse> listener)
        {
            int cnt = _listeners.Count(x => x.Key == key && x.Value == listener);
            Log.Info("Unregistering api listener for key '{0}', found {1} item{2}", key, cnt, (cnt != 1 ? "s" : ""));
            if (cnt > 0)
            {
                _listeners.RemoveAll(x => x.Key == key && x.Value == listener);

                foreach (var item in apis)
                {
                    item.OnListenerRemoved(key);
                    item.OnListenerRemoved(key, listener);
                }
            }
        }

        public static void OnShutdown()
        {
            foreach (var item in apis)
            {
                item.OnShutdown();
            }
        }
    }
}