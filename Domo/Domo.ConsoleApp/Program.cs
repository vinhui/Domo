using Domo.API;
using System;

namespace Domo.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ApiManager.Init();

            Console.ReadKey();
            ApiManager.OnShutdown();

            Console.ReadKey();
        }
    }
}