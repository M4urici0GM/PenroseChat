using System;

namespace Penrose.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string name, object key)
            : base($"Entity of type {name} with key {key} wasn't found.")
        {
            
        }
    }
}