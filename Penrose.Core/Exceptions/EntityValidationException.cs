using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace Penrose.Core.Exceptions
{
    public class EntityValidationException : Exception
    {
        public IEnumerable<ValidationFailure> ValidationErrors { get; set; }

        public EntityValidationException(string name, object key, IEnumerable<ValidationFailure> errors)
            : base ($"The validation for entity type {name} failed.")
        {
            ValidationErrors = errors;
        }
    }
}