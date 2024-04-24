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
    public class MonthToStringConverter : IValueConverter
    {
        static readonly string[] _engMonths = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int month = (int)value;
            if(month < 1 || month > 12)
                throw new ArgumentException("Value must be an integer between 1 and 12");

            if (String.Compare(culture.Name, "ko-KR", true) == 0)
            {
                return $"{month}월";
            }
            else
            {
                return _engMonths[month - 1];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
