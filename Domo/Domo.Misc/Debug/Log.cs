using System;
using System.Collections.Generic;

namespace Domo.Misc.Debug
{
    /// <summary>
    /// Main log class
    /// </summary>
    public static class Log
    {
        private static List<Logger> loggers = new List<Logger>();

        static Log()
        {
            RegisterLogger<ConsoleLogger>();
            RegisterLogger<FileLogger>();
        }

        /// <summary>
        /// Register a new logger of type <paramref name="T"/>
        /// The type has to derive from <see cref="Logger"/>
        /// </summary>
        /// <typeparam name="T">The type to register</typeparam>
        public static void RegisterLogger<T>() where T : Logger
        {
            RegisterLogger(typeof(T));
        }

        /// <summary>
        /// Register a new logger of type <paramref name="T"/>
        /// The type has to derive from <see cref="Logger"/>
        /// </summary>
        /// <param name="type">The type to register</param>
        public static void RegisterLogger(Type type)
        {
            if (!type.IsSubclassOf(typeof(Logger)))
            {
                Log.Error("The logger you are trying to register (" + type.FullName + ") does not derive from '" + typeof(Logger).FullName + "'!");
            }

            Logger instance = Activator.CreateInstance(type) as Logger;
            if (instance != null)
            {
                Log.Debug("Registering logger '" + type.FullName + "'");
                loggers.Add(instance);
            }
            else
                Log.Error("Failed to create an instance of '" + type.FullName + "'!");
        }

        /// <summary>
        /// Unregister a logger
        /// </summary>
        /// <typeparam name="T">Type of the logger to unregister</typeparam>
        public static void UnregisterLogger<T>() where T : Logger
        {
            UnregisterLogger(typeof(T));
        }

        /// <summary>
        /// Unregister a logger
        /// </summary>
        /// <param name="type">Type of the logger to unregister</param>
        public static void UnregisterLogger(Type type)
        {
            Log.Debug("Unregistering logger '" + type.FullName + "'");

            loggers.RemoveAll(x => x.GetType() == type);
        }

        public static void OnShutDown()
        {
            foreach (var item in loggers)
                item.OnShutDown();
        }

        #region Log Debug

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="msg">Message to log</param>
        /// <param name="args"><see cref="string.Format(string, object[])"/></param>
        public static void Debug(string msg, params object[] args)
        {
            Debug(string.Format(msg, args));
        }

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="msg">Message to log</param>
        public static void Debug(object msg)
        {
            Debug(msg.ToString());
        }

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="msg">Message to log</param>
        public static void Debug(string msg)
        {
            foreach (var item in loggers)
                item.LogDebug(msg);
        }

        #endregion Log Debug

        #region Log Info

        /// <summary>
        /// Log an info message
        /// </summary>
        /// <param name="msg">Message to log</param>
        /// <param name="args"><see cref="string.Format(string, object[])"/></param>
        public static void Info(string msg, params object[] args)
        {
            Info(string.Format(msg, args));
        }

        /// <summary>
        /// Log an info message
        /// </summary>
        /// <param name="msg">Message to log</param>
        public static void Info(object msg)
        {
            Info(msg.ToString());
        }

        /// <summary>
        /// Log an info message
        /// </summary>
        /// <param name="msg">Message to log</param>
        public static void Info(string msg)
        {
            foreach (var item in loggers)
                item.LogInfo(msg);
        }

        #endregion Log Info

        #region Log Warning

        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="msg">Message to log</param>
        /// <param name="args"><see cref="string.Format(string, object[])"/></param>
        public static void Warning(string msg, params object[] args)
        {
            Warning(string.Format(msg, args));
        }

        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="msg">Message to log</param>
        public static void Warning(object msg)
        {
            Warning(msg.ToString());
        }

        /// <summary>
        /// Log a warning message
        /// </summary>
        /// <param name="msg">Message to log</param>
        public static void Warning(string msg)
        {
            foreach (var item in loggers)
                item.LogWarning(msg);
        }

        #endregion Log Warning

        #region Log Error

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="msg">Message to log</param>
        /// <param name="args"><see cref="string.Format(string, object[])"/></param>
        public static void Error(string msg, params object[] args)
        {
            Error(string.Format(msg, args));
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="msg">Message to log</param>
        public static void Error(object msg)
        {
            Error(msg.ToString());
        }

        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="msg">Message to log</param>
        public static void Error(string msg)
        {
            foreach (var item in loggers)
                item.LogError(msg);
        }

        #endregion Log Error

        #region Log Exception

        /// <summary>
        /// Log an exception
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static void Exception(Exception ex)
        {
            foreach (var item in loggers)
                item.LogException(ex);
        }

        #endregion Log Exception
    }
}