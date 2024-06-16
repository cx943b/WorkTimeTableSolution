using CommunityToolkit.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WorkTimeTable.Infrastructure.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumToStringConverter : IValueConverter
    {   
        public object Convert(object oValue, Type targetType, object parameter, CultureInfo culture)
        {
            if(oValue == null)
                throw new ArgumentNullException(nameof(oValue));

            if(parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            Type enumType = (Type)parameter;
            if(!enumType.IsEnum)
                throw new ArgumentException("Parameter must be an enum type");

            if (enumType.GetCustomAttribute<FlagsAttribute>() != null)
            {
                List<string> lstEnumString = new List<string>();
                Enum eValue = (Enum)oValue;

                foreach (FieldInfo fInfo in enumType.GetFields())
                {
                    if(Enum.TryParse(enumType, fInfo.Name, out object? oEnum))
                    {
                        var targetEnum = (Enum)oEnum;
                        if (!eValue.HasFlag(targetEnum))
                            continue;

                        DescriptionAttribute? descAtt = fInfo.GetCustomAttribute<DescriptionAttribute>();
                        if (descAtt != null)
                            lstEnumString.Add(descAtt.Description);
                        else
                            lstEnumString.Add(fInfo.Name);
                    }
                }

                return String.Join(", ", lstEnumString);
            }
            else
            {
                string? sValue = oValue.ToString();
                if(String.IsNullOrEmpty(sValue))
                    throw new ArgumentException("Value must be a string");

                DescriptionAttribute? descAtt = enumType
                    .GetField(sValue)?
                    .GetCustomAttribute<DescriptionAttribute>();

                if (descAtt != null)
                    return descAtt.Description;
                else
                    return sValue;
            }
        }

        public object ConvertBack(object oValue, Type targetType, object parameter, CultureInfo culture)
        {
            string? sValue = oValue?.ToString();
            if (sValue == null)
                throw new ArgumentNullException(nameof(oValue));

            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            Type enumType = (Type)parameter;
            if (!enumType.IsEnum)
                throw new ArgumentException("Parameter must be an enum type");
            
            if (enumType.GetCustomAttribute<FlagsAttribute>() != null)
            {
                List<int> lstEnumValues = new List<int>();
                string[] enumStrings = sValue.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                if(enumStrings.Any())
                {
                    foreach(var enumString in enumStrings)
                    {
                        if(Enum.TryParse(enumType, enumString, true, out object? enumValue))
                        {
                            lstEnumValues.Add((int)enumValue);
                        }
                        else
                        {
                            var fieldInfo = enumType
                                .GetFields()
                                .Where(fi =>
                                {
                                    var descAtt = fi.GetCustomAttribute<DescriptionAttribute>();
                                    if (descAtt != null)
                                        return String.Compare(descAtt.Description, enumString, true) == 0;
                                    else
                                        return false;
                                })
                                .FirstOrDefault();

                            if (fieldInfo != null)
                                lstEnumValues.Add((int)Enum.Parse(enumType, fieldInfo.Name));
                        }
                    }

                    if(lstEnumValues.Any())
                    {
                        return lstEnumValues.Aggregate(default(int), (c, n) => c | n);
                    }

                    throw new InvalidEnumArgumentException("Value must be an enum value", 0, enumType);
                }
            }

            if (String.IsNullOrEmpty(sValue))
                throw new ArgumentException("Value must be a string");

            foreach (FieldInfo fInfo in enumType.GetFields())
            {
                if (String.Compare(fInfo.Name, sValue, true) == 0)
                {
                    return Enum.Parse(enumType, fInfo.Name);
                }

                DescriptionAttribute? descAtt = fInfo.GetCustomAttribute<DescriptionAttribute>();
                if (descAtt != null && String.Compare(descAtt.Description, sValue, true) == 0)
                    return Enum.Parse(enumType, descAtt.Description);
            }

            throw new InvalidEnumArgumentException("Value must be an enum value", 0, enumType);
        }
    }
}
