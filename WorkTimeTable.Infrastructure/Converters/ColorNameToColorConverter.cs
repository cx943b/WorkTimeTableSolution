using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorkTimeTable.Infrastructure.Converters
{
    public class ColorNameToColorConverter : IValueConverter
    {
        static readonly string[] _colorNames = typeof(System.Windows.Media.Colors).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Select(c => c.Name).ToArray();
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string? colorName = value as string;
            if (String.IsNullOrEmpty(colorName))
                throw new ArgumentNullException(nameof(value));

            if(Array.IndexOf(_colorNames, colorName) > 0)
                return System.Windows.Media.ColorConverter.ConvertFromString(colorName);
            else
                throw new InvalidOperationException("Invalid color name");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
