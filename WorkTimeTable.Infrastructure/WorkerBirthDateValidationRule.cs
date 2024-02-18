using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkTimeTable.Infrastructure
{
    public class WorkerBirthDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? sValue = value as string;
            if(String.IsNullOrEmpty(sValue))
                return new ValidationResult(false, "BirthDate cannnot be empty");

            if(DateTime.TryParseExact(sValue, "yyMMdd", null, DateTimeStyles.None, out DateTime birthDate))
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Invalid BirthDate");
        }
    }
}
