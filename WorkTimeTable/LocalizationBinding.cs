using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Services;

namespace WorkTimeTable
{
    public class LocalizationBinding : Binding
    {
        static LocalizationSource _locSrc = new LocalizationSource();

        public static CultureInfo TargetCulture
        {
            get => _locSrc.CurrentCulture;
            set => _locSrc.CurrentCulture = value;
        }


        string _LocalizationKey ="";
        public string LocalizationKey
        {
            get => _LocalizationKey;
            set
            {
                if (value != _LocalizationKey)
                {
                    _LocalizationKey = value;
                    onLocalizationChanged();
                }
            }
        }

        public LocalizationBinding() => Source = _locSrc;


        private void onLocalizationChanged()
        {
            if (String.IsNullOrEmpty(_LocalizationKey))
            {
                this.Path = null;
            }
            else
            {
                this.Path = new PropertyPath(_LocalizationKey);
            }
        }
    }
}
