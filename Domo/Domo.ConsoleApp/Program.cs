using Domo.API;
using Domo.Misc;
using Domo.Misc.Debug;
using System;

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
            ApiManager.Init();
            ApiManager.RegisterListener("test", handler);

            Console.ReadKey();
            ApiManager.OnShutdown();

            Console.ReadKey();
        }

        private static ApiResponse handler(ApiListenerData data)
        {
            return new ApiResponse()
            {
                code = 0,
                success = true,
                data = new System.Collections.Generic.Dictionary<string, object>(data.arguments)
            };
        }
    }
}