using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WorkTimeTable.Infrastructure
{
    public class BarTimeValidationRule : ValidationRule
    {
        DependencyObject? _bindingTarget;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(true, null);
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            _bindingTarget = owner.Target;
            return new ValidationResult(true, null);
        }
    }
}
