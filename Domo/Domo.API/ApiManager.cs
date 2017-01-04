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

        private static Dictionary<string, Func<ApiRequest, ApiResponse>> _listeners;
        public static IReadOnlyDictionary<string, Func<ApiRequest, ApiResponse>> listeners { get { return _listeners; } }

        public static void Init()
        {
            _apis = new List<ApiBase>();
            _listeners = new Dictionary<string, Func<ApiRequest, ApiResponse>>();

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

        public static void RegisterListener(string key, Func<ApiRequest, ApiResponse> listener)
        {
            if (!_listeners.ContainsKey(key))
            {
                Log.Info("Registering API listener for key '{0}'", key);
                _listeners.Add(key, listener);

                foreach (var item in apis)
                {
                    item.OnListenerRegistered(key);
                    item.OnListenerRegistered(key, listener);
                }
            }
            else
            {
                throw new KeyNotFoundException("There is already a listener registered for key '" + key + "'");
            }
        }

        public static void UnregisterListener(string key)
        {
            if (!_listeners.ContainsKey(key))
            {
                Func<ApiRequest, ApiResponse> listener = _listeners[key];
                _listeners.Remove(key);
                Log.Info("Removing API listener for key '{0}'", key);

                foreach (var item in apis)
                {
                    item.OnListenerRemoved(key);
                    item.OnListenerRemoved(key, listener);
                }
            }
            else
            {
                throw new KeyNotFoundException("There is no listener registered for key '" + key + "'");
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