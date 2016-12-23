using System;

namespace Domo.Debug
{
    public abstract class Logger
    {
        public virtual void LogDebug(string line)
        {
            Log(LogType.Debug, line);
        }

        public virtual void LogInfo(string line)
        {
            Log(LogType.Info, line);
        }

        public virtual void LogWarning(string line)
        {
            Log(LogType.Warning, line);
        }

        public virtual void LogError(string line)
        {
            Log(LogType.Error, line);
        }

        public virtual void LogException(Exception ex)
        {
            Log(LogType.Error, ex.GetType() + ": " + ex.Message);
            Log(LogType.Error, ex.StackTrace);
        }

        public abstract void Log(LogType type, string msg);

        public enum LogType
        {
            Debug = -1,
            Info = 0,
            Warning = 1,
            Error = 2,
        }
    }
}