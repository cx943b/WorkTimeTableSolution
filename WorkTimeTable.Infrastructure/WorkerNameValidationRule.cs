using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkTimeTable.Infrastructure
{
    public class WorkerNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? sValue = value as string;
            if (String.IsNullOrEmpty(sValue))
                return new ValidationResult(false, "Name cannot be empty");

            return new ValidationResult(true, null);
        }
    }
}
