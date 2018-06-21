using System;

namespace Domain.Core.Exceptions
{
    public class KeyExistsException : ApplicationException
    {
        public KeyExistsException(string message) : base(message)
        {
        }
    }
}
