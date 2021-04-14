using System;

namespace Penrose.Core.Exceptions
{
    public class RedisEntityException : Exception
    {
        public RedisEntityException(string entity, object key, string message)
            : base($"The error \"{message}\" was thrown when handling entity {entity} with key({message}).")
        {
        }
    }
}