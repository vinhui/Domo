using System;

namespace Domo.ConsoleApp
{
    internal class Program
    {
        private static bool isShuttingDown = false;

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Domo.Main.Init();

            Console.ReadKey();
            isShuttingDown = true;
            Domo.Main.ShutDown();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (!isShuttingDown)
                Domo.Main.ShutDown();
        }
    }
}