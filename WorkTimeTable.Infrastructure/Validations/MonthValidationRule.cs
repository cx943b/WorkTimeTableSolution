using System.Globalization;
using System.Windows.Controls;

namespace WorkTimeTable.Infrastructure.Validations
{
    public class MonthValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? sValue = value as string;
            if (string.IsNullOrEmpty(sValue))
                return new ValidationResult(false, "Month cannot be empty");

            if (int.TryParse(sValue, out int month))
            {
                if (month < 1)
                    return new ValidationResult(false, "Month cannot be under 1");
                else if (month > 12)
                    return new ValidationResult(false, "Month cannot be over 12");
            }
            else
            {
                return new ValidationResult(false, "Invalid Month");
            }

            return new ValidationResult(true, null);
        }
    }
}
