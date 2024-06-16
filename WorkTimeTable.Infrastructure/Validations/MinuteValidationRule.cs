using System.Globalization;
using System.Windows.Controls;

namespace WorkTimeTable.Infrastructure.Validations
{
    public class MinuteValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? sValue = value as string;
            if (string.IsNullOrEmpty(sValue))
                return new ValidationResult(false, "Minute cannot be empty");

            if (int.TryParse(sValue, out int minute))
            {
                if (minute < 0)
                    return new ValidationResult(false, "Minute cannot be under 0");
                else if (minute > 59)
                    return new ValidationResult(false, "Minute cannot be over 59");
            }
            else
            {
                return new ValidationResult(false, "Invalid Minute");
            }

            return new ValidationResult(true, null);
        }
    }
}
