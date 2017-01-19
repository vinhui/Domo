using System;

namespace Domo.Misc.Debug
{
    internal class ConsoleLogger : Logger
    {
        public override void Log(LogType type, string msg)
        {
            switch (type)
            {
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }

            string t = type.ToString();
            Console.WriteLine(("[" + t + "]").PadRight(8, ' ') + ": " + msg);
        }
    }
}