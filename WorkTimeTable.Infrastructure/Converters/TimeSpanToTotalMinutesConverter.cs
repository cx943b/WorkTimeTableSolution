using CommunityToolkit.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorkTimeTable.Infrastructure.Converters
{
    public class TimeSpanToTotalMinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return 0;

            // check value type is timespan
            if(value is not TimeSpan)
                throw new ArgumentException("Value must be a TimeSpan");

            TimeSpan span = (TimeSpan)value;
            return span.TotalMinutes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
                return TimeSpan.Zero;

            if (value is not double)
                throw new ArgumentException("Value must be an Int");

            return TimeSpan.FromMinutes((double)value);
        }
    }
}
