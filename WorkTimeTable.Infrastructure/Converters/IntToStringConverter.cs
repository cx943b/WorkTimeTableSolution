using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorkTimeTable.Infrastructure.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || !(value is int nValue))
                throw new ArgumentException("Value must be an integer");

            return nValue.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                throw new ArgumentException($"NullRef: {nameof(value)}");

            if (!Int32.TryParse(value.ToString(), out int result))
                throw new ArgumentException("Value must be an integer string");

            return result;
        }
    }
}
