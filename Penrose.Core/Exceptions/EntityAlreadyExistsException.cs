using System;

namespace Penrose.Core.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string name, object key)
            : base($"There's already one entity of type {name} with key {key}")
        {
            
        }
    }
}