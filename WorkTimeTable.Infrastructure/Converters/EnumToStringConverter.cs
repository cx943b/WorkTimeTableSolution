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

                        DescriptionAttribute descAtt = fInfo.GetCustomAttribute<DescriptionAttribute>();
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
                DescriptionAttribute descAtt = enumType.GetField(oValue.ToString()).GetCustomAttribute<DescriptionAttribute>();
                if (descAtt != null)
                    return descAtt.Description;
                else
                    return oValue.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? sValue = value?.ToString();
            if (sValue == null)
                throw new ArgumentNullException(nameof(value));

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
                        if(Enum.TryParse(enumType, enumString, true, out object enumValue))
                        {
                            lstEnumValues.Add((int)enumValue);
                        }
                        else
                        {
                            var fieldInfo = enumType
                                .GetFields()
                                .Where(fi =>
                                {
                                    if (fi.GetCustomAttribute<DescriptionAttribute>() != null)
                                        return String.Compare(fi.GetCustomAttribute<DescriptionAttribute>().Description, enumString, true) == 0;
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

            foreach (FieldInfo fInfo in enumType.GetFields())
            {
                if (String.Compare(fInfo.Name, value.ToString(), true) == 0)
                {
                    return Enum.Parse(enumType, fInfo.Name);
                }

                DescriptionAttribute descAtt = fInfo.GetCustomAttribute<DescriptionAttribute>();
                if (descAtt != null && String.Compare(descAtt.Description, value.ToString(), true) == 0)
                    return Enum.Parse(enumType, descAtt.Description);
            }

            throw new InvalidEnumArgumentException("Value must be an enum value", 0, enumType);
        }
    }
}
