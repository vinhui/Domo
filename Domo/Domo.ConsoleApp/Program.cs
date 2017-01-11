using Domo.API;
using System;

namespace Domo.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Domo.Main.Init();


            Console.ReadKey();
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Domo.Main.ShutDown();
        }
    }
}