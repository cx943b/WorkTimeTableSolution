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
            if (Int32.TryParse(value.ToString(), out int result))
                return value.ToString();

            throw new ArgumentException("Value must be an integer");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (Int32.TryParse(value.ToString(), out int result))
                return result;

            throw new ArgumentException("Value must be an integer string");
        }
    }
}
