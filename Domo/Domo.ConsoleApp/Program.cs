using Domo.API;
using System;

namespace Domo.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ApiManager.Init();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Console.ReadKey();
            ApiManager.OnShutdown();

            Console.ReadKey();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            ApiManager.OnShutdown();
        }
    }
}