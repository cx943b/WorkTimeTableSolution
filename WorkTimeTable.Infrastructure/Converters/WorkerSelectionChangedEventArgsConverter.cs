using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using WorkTimeTable.Infrastructure.Interfaces;

namespace WorkTimeTable.Infrastructure.Converters
{
    public class WorkerSelectionChangedEventArgsConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var args = value as SelectionChangedEventArgs;
            if (args == null)
                return null;

            if(args.AddedItems is not null && args.AddedItems.Count > 0)
            {
                return args.AddedItems[0] as IWorker;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
