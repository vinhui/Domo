using System;

namespace Domo.Debug
{
    internal class ConsoleLogger : Logger
    {
        public override void Log(LogType type, string msg)
        {
            Console.WriteLine(type.ToString() + ": " + msg);
        }
    }
}