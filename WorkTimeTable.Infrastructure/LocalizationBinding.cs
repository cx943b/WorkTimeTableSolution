using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WorkTimeTable.Infrastructure
{
    public class LocalizationBinding : Binding
    {
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
