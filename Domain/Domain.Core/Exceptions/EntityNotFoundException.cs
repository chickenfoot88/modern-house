using System;

namespace Domain.Core.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
