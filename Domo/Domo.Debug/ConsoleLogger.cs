using System;

namespace Domo.Debug
{
    internal class ConsoleLogger : Logger
    {
        public override void Log(LogType type, string msg)
        {
            string t = type.ToString();
            Console.WriteLine(("[" + t + "]").PadRight(8, ' ') + ": " + msg);
        }
    }
}