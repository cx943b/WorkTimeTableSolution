using System.Globalization;
using System.Windows.Controls;

namespace WorkTimeTable.Infrastructure.Validations
{
    public class DayValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? sValue = value as string;
            if (string.IsNullOrEmpty(sValue))
                return new ValidationResult(false, "Day cannot be empty");

            if (int.TryParse(sValue, out int day))
            {
                if (day < 1)
                    return new ValidationResult(false, "Day cannot be under 1");
                else if (day > 31)
                    return new ValidationResult(false, "Day cannot be over 31");
            }
            else
            {
                return new ValidationResult(false, "Invalid Day");
            }

            return new ValidationResult(true, null);
        }
    }
}
