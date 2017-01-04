using System;

namespace Domo.Misc.Debug
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