using System;
using System.Collections.Generic;

namespace Domo.API
{
    public abstract class ApiBase
    {
        public IEnumerable<KeyValuePair<string, Func<ApiListenerData, ApiResponse>>> listeners
        {
            get { return ApiManager.listeners; }
        }

        public abstract void Init();

        public virtual void OnListenerRegistered(string key)
        {
        }

        public virtual void OnListenerRegistered(string key, Func<ApiListenerData, ApiResponse> listener)
        {
        }

        public virtual void OnListenerRemoved(string key)
        {
        }

        public virtual void OnListenerRemoved(string key, Func<ApiListenerData, ApiResponse> listener)
        {
        }

        public virtual void OnShutdown()
        {
        }
    }
}