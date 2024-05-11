using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        static readonly string[] _monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int nMonth = (int)value;
            if(nMonth < 1 || nMonth > 12)
                throw new ArgumentOutOfRangeException("Month must be between 1 and 12");

            if (String.Compare(culture.Name, "ko-KR") == 0)
                return $"{nMonth}월";
            else
                return _monthNames[nMonth - 1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MonthConverter.ConvertBack is not supported");
        }
    }
}
