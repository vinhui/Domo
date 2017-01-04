using System;

namespace Domo.Misc.Debug
{
    /// <summary>
    /// Base class for new loggers
    /// </summary>
    public abstract class Logger
    {
        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="line">Message to log</param>
        public virtual void LogDebug(string line)
        {
            Log(LogType.Debug, line);
        }

        /// <summary>
        /// Log an info message
        /// </summary>
        /// <param name="line">Message to log</param>
        public virtual void LogInfo(string line)
        {
            Log(LogType.Info, line);
        }

        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="line">Message to log</param>
        public virtual void LogWarning(string line)
        {
            Log(LogType.Warning, line);
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="line">Message to log</param>
        public virtual void LogError(string line)
        {
            Log(LogType.Error, line);
        }

        /// <summary>
        /// Log an exception
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public virtual void LogException(Exception ex)
        {
            Log(LogType.Error, ex.GetType() + ": " + ex.Message);
            Log(LogType.Error, ex.StackTrace);
        }

        /// <summary>
        /// Global log function all the other functions call
        /// </summary>
        /// <param name="type">Type of the message</param>
        /// <param name="msg">Message to log</param>
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