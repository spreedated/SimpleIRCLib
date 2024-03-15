using System;

namespace SimpleIRCLib.Exceptions
{
    public class InvalidPortException : Exception
    {
        public InvalidPortException() : base()
        {
        }

        public InvalidPortException(string message) : base(message)
        {
        }
    }
}
