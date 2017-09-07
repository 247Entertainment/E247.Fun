using System;

#pragma warning disable 1591

namespace E247.Fun.Exceptions
{
    public sealed class FailedMatchException : Exception
    {
        const string message = 
            "Somehow, none of the cases were matched on this choice, this is " +
            "spooky and should never happen. Good luck debugging!";

        public FailedMatchException() : base(message){}

        public FailedMatchException(Exception innerException) 
            : base(message, innerException){}
    }
}
