namespace Penrose.Core.Structs
{
    public struct ValidatorDefaultErrorMessages
    {
        public const string RequiredField = "The field {propertyName} is required!";
        public const string LengthNotSatisfied = "The field  {PropertyName} need to have {MaxLength} characters.";

        public const string IntervalLengthNotSatisfied =
            "The field {PropertyName} need to have between {MinLength} and {MaxLength} characters.";

        public const string MaxLengthNotSatisfied = "The field {PropertyName} can have at maximum {MaxLength} characters.";

        public const string MinLengthNotSatisfied =
            "The field {PropertyName} need to have at least {MinLength} characters.";
    }
    
    
}