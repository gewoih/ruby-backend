using System.ComponentModel.DataAnnotations;

namespace Casino.SharedLibrary.Attributes
{
    public sealed class NotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is double number)
                return number != 0;

            if (value is Guid guid)
                return guid != Guid.Empty;

            if (value is string text)
                return !string.IsNullOrEmpty(text);

            return false;
        }
    }
}