using System;

#pragma warning disable 1591

namespace E247.Fun.Exceptions
{
    public sealed class ResultAccessException : Exception
    {
        public ResultAccessException(string message) : base(message){}
    }
}
