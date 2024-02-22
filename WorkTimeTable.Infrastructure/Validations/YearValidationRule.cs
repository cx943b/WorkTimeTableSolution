using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkTimeTable.Infrastructure.Validations
{
    public class YearValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? sValue = value as string;
            if (string.IsNullOrEmpty(sValue))
                return new ValidationResult(false, "Year cannot be empty");

            if (int.TryParse(sValue, out int year))
            {
                if(year < DateTime.MinValue.Year)
                    return new ValidationResult(false, "Year cannot be under MinYear");
                else if (year > DateTime.MaxValue.Year)
                    return new ValidationResult(false, "Year cannot be over MaxYear");
            }
            else
            {
                return new ValidationResult(false, "Invalid Year");
            }

            return new ValidationResult(true, null);
        }
    }
}