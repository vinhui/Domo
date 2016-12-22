using System;
using System.Collections.Generic;

namespace Domo.Debug
{
    public static class Log
    {
        private static List<Logger> loggers = new List<Logger>();

        static Log()
        {
            RegisterLogger<ConsoleLogger>();
        }

        public static void RegisterLogger<T>() where T : Logger
        {
            RegisterLogger(typeof(T));
        }

        public static void RegisterLogger(Type type)
        {
            if (!type.IsSubclassOf(typeof(Logger)))
            {
                Log.Error("The logger you are trying to register (" + type.FullName + ") does not derive from '" + typeof(Logger).FullName + "'!");
            }

            Logger instance = Activator.CreateInstance(type) as Logger;
            if (instance != null)
                loggers.Add(instance);
            else
                Log.Error("Failed to create an instance of '" + type.FullName + "'!");
        }

        #region Log Debug

        public static void Debug(string msg, params object[] args)
        {
            Debug(string.Format(msg, args));
        }

        public static void Debug(object msg)
        {
            Debug(msg.ToString());
        }

        public static void Debug(string msg)
        {
            foreach (var item in loggers)
                item.LogDebug(msg);
        }

        #endregion Log Debug

        #region Log Info

        public static void Info(string msg, params object[] args)
        {
            Info(string.Format(msg, args));
        }

        public static void Info(object msg)
        {
            Info(msg.ToString());
        }

        public static void Info(string msg)
        {
            foreach (var item in loggers)
                item.LogInfo(msg);
        }

        #endregion Log Info

        #region Log Warning

        public static void Warning(string msg, params object[] args)
        {
            Warning(string.Format(msg, args));
        }

        public static void Warning(object msg)
        {
            Warning(msg.ToString());
        }

        public static void Warning(string msg)
        {
            foreach (var item in loggers)
                item.LogWarning(msg);
        }

        #endregion Log Warning

        #region Log Error

        public static void Error(string msg, params object[] args)
        {
            Error(string.Format(msg, args));
        }

        public static void Error(object msg)
        {
            Error(msg.ToString());
        }

        public static void Error(string msg)
        {
            foreach (var item in loggers)
                item.LogError(msg);
        }

        #endregion Log Error

        #region Log Exception

        public static void Exception(Exception ex)
        {
            foreach (var item in loggers)
                item.LogException(ex);
        }

        #endregion Log Exception
    }
}