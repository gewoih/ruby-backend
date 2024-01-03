using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Casino.SharedLibrary.Attributes
{
    public sealed class NotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is Guid guid)
                return guid != Guid.Empty;

            if (value is string text)
                return !string.IsNullOrEmpty(text);
            
            if (value is IConvertible convertible)
            {
				var convertedValue = convertible.ToDouble(CultureInfo.CurrentCulture);
                return !convertedValue.Equals(0);
			}

            return false;
        }
    }
}