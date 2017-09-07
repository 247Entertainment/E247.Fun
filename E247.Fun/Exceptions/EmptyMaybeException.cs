using System;

#pragma warning disable 1591

namespace E247.Fun.Exceptions
{
    public sealed class EmptyMaybeException : Exception
    {
        const string message = 
            "Attempted to access the value of a Maybe<T> when it was empty, " +
            "you must ALWAYS check for a value before attempting to access it.";

        public EmptyMaybeException() : base(message){}

        public EmptyMaybeException(Exception innerException) 
            : base(message, innerException){}
    }
}
