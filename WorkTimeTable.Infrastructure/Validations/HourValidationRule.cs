using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkTimeTable.Infrastructure.Validations
{
    public class HourValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? sValue = value as string;
            if (string.IsNullOrEmpty(sValue))
                return new ValidationResult(false, "Hour cannot be empty");

            if (int.TryParse(sValue, out int hour))
            {
                if (hour < 0)
                    return new ValidationResult(false, "Hour cannot be under 0");
                else if (hour > 23)
                    return new ValidationResult(false, "Hour cannot be over 23");
            }
            else
            {
                return new ValidationResult(false, "Invalid Hour");
            }

            return new ValidationResult(true, null);
        }
    }
}