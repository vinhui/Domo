using System;

namespace Domo.Modules
{
    public class NotInitializedException : Exception
    {
        public NotInitializedException()
        {
        }

        public NotInitializedException(string message)
            : base(message)
        {
        }

        public NotInitializedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}