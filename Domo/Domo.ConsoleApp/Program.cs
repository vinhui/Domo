using Domo.Main;
using Domo.Main.Debug;
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
            Console.ReadKey();
        }
    }
}