using Domo.API;
using Domo.API.Web;
using Domo.Misc;
using Domo.Misc.Debug;
using System;
using System.Linq;

namespace Domo.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
            Log.Debug("Confirming config works correctly");
            Config.SetValue(true, false, "test", "test");

            if (Config.GetValue<bool>("test", "test"))
                Log.Debug("Config works properly");
            else
                Log.Error("Config doesn't work properly!");
#endif
            IApiBase api = new WebAPI();
            api.Init();
            api.RegisterListener("test", handler);

            Console.ReadKey();
            api.OnShutdown();

            Console.ReadKey();
        }

        private static ApiResponse handler(ApiListenerData data)
        {
            Log.Debug("Received data for key '{0}'", data.key);

            return new ApiResponse()
            {
                code = 0,
                success = true,
                data = new System.Collections.Generic.Dictionary<string, object>(data.arguments)
            };
        }
    }
}