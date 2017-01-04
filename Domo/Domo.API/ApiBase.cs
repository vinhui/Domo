using System;
using System.Collections.Generic;

namespace Domo.API
{
    public abstract class ApiBase
    {
        public IEnumerable<KeyValuePair<string, Func<ApiRequest, ApiResponse>>> listeners
        {
            get { return ApiManager.listeners; }
        }

        /// <summary>
        /// Initializer that automatically gets called when the API added by the manager
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Gets called when a listener is registered
        /// </summary>
        /// <param name="key">The key the listener is registered with</param>
        public virtual void OnListenerRegistered(string key)
        {
        }

        /// <summary>
        /// Gets called when a listener is registered
        /// </summary>
        /// <param name="key">The key the listener is registered with</param>
        /// <param name="listener">The listener it self</param>
        public virtual void OnListenerRegistered(string key, Func<ApiRequest, ApiResponse> listener)
        {
        }

        /// <summary>
        /// Gets called when a listener is removed
        /// </summary>
        /// <param name="key">The key of the listener that's removed</param>
        public virtual void OnListenerRemoved(string key)
        {
        }

        /// <summary>
        /// Gets called when a listener is removed
        /// </summary>
        /// <param name="key">The key of the listener that's removed</param>
        /// <param name="listener"></param>
        public virtual void OnListenerRemoved(string key, Func<ApiRequest, ApiResponse> listener)
        {
        }

        /// <summary>
        /// Gets called when the program gets shut down
        /// </summary>
        public virtual void OnShutdown()
        {
        }
    }
}