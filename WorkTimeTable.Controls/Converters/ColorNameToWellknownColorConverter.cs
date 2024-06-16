using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using SosoThemeLibrary;

namespace WorkTimeTable.Controls.Converters
{
    [ValueConversion(typeof(string), typeof(WellknownColor))]
    public class ColorNameToWellknownColorConverter : IValueConverter
    {
        static string[] _colorNames = typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static).Select(c => c.Name).ToArray();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string? colorName = value as string;
            if (String.IsNullOrEmpty(colorName))
                return null;
                
            if(_colorNames.Any(cn => String.Compare(cn, colorName, true) == 0))
                return new WellknownColor(colorName, (Color)ColorConverter.ConvertFromString(colorName));

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            WellknownColor? wkColor = value as WellknownColor;

            if(wkColor != null)
                return wkColor.Name;
            else
                return null;
        }
    }
}
