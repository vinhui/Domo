using Domo.Misc.Debug;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Domo.API
{
    public static class ApiManager
    {
        private static List<ApiBase> _apis;

        /// <summary>
        /// All the registered API handlers
        /// </summary>
        public static IEnumerable<ApiBase> apis { get { return _apis; } }

        private static Dictionary<string, Func<ApiRequest, ApiResponse>> _listeners;

        /// <summary>
        /// All the registered listeners
        /// </summary>
        public static IReadOnlyDictionary<string, Func<ApiRequest, ApiResponse>> listeners { get { return _listeners; } }

        /// <summary>
        /// Initialize the API manager and APIs
        /// If already initialzed, this will reset
        /// </summary>
        public static void Init()
        {
            _apis = new List<ApiBase>();
            _listeners = new Dictionary<string, Func<ApiRequest, ApiResponse>>();

            GetApis();

            foreach (var item in apis)
                item.Init();
        }

        /// <summary>
        /// Get all the APIs from the current appdomain
        /// </summary>
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
                            Log.Debug("Found API '{0}' in assembly '{1}'", type.Name, assembly.FullName);
                            cnt++;
                        }
                    }
                }
            }

            Log.Info("Found {0} API{1}", cnt, (cnt > 1 || cnt == 0) ? "s" : "");
        }

        /// <summary>
        /// Register a new API listener
        /// You can't register multiple listeners for one key
        /// </summary>
        /// <param name="key">The key to register a listener for</param>
        /// <param name="listener">The listener that gets called</param>
        /// <exception cref="InvalidOperationException">Thrown when the key is already registered</exception>
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
                throw new InvalidOperationException("There is already a listener registered for key '" + key + "'");
            }
        }

        /// <summary>
        /// Unregister a listener so that it wont get new calls
        /// </summary>
        /// <param name="key">The key to unregister for</param>
        /// <exception cref="KeyNotFoundException">Throws an exception when the key provided is not registered</exception>
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

        /// <summary>
        /// This should be called when the application shuts down
        /// </summary>
        public static void OnShutdown()
        {
            if (apis == null)
                return;

            foreach (var item in apis)
            {
                item.OnShutdown();
            }
        }
    }
}