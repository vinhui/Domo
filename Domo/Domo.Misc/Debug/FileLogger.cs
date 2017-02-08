using System;
using System.IO;

namespace Domo.Misc.Debug
{
    internal class FileLogger : Logger
    {
        private const string logFileName = "ouput.txt";
        private StreamWriter writer;

        public FileLogger()
        {
            writer = new StreamWriter(logFileName, true);
            writer.AutoFlush = true;
        }

        internal override void OnShutDown()
        {
            writer.Dispose();
        }

        public override void Log(LogType type, string msg)
        {
            string t = type.ToString();
            writer.WriteLine(("[" + t + "]").PadRight(8, ' ') + ": " + msg);
        }
    }
}