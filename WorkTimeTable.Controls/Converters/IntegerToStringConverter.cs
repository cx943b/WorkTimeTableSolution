using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorkTimeTable.Controls.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntegerToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            return value.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;

            string sValue = value.ToString();
            if (Int32.TryParse(sValue, out int nValue))
                return nValue;

            return 0;
        }
    }
}
