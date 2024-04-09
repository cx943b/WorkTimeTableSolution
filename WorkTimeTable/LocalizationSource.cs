using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable
{
    public class LocalizationSource : DynamicObject, INotifyPropertyChanged
    {
        public const string StartWorkTime = "StartWorkTime";
        public const string EndWorkTime = "EndWorkTime";
        public const string WorkTimeSpan = "WorkTimeSpan";
        public const string Year = "Year";
        public const string Month = "Month";
        public const string Day = "Day";
        public const string Hour = "Hour";
        public const string Minute = "Minute";
        public const string Week = "Week";

        CultureInfo _CurrentCulture = CultureInfo.CurrentCulture;

        readonly Dictionary<string, string> _dicLabelKor = new Dictionary<string, string>();
        readonly Dictionary<string, string> _dicLabelEng = new Dictionary<string, string>();

        public CultureInfo CurrentCulture
        {
            get => _CurrentCulture;
            set
            {
                if (value != _CurrentCulture)
                {
                    _CurrentCulture = value;
                    onCurrentCultureChanged();
                }
            }
        }

        public LocalizationSource() => initLabels();

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            if (value == null || value is not string)
                return false;

            string key = binder.Name;
            string sValue = (string)value;
            Dictionary<string, string> dicTargetLabel = string.Compare(CurrentCulture.Name, "ko-KR", true) == 0 ? _dicLabelKor : _dicLabelEng;

            if (dicTargetLabel.ContainsKey(key))
                dicTargetLabel[key] = sValue;
            else
                dicTargetLabel.Add(key, sValue);

            onPropertyChanged(binder.Name);
            return true;
        }
        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            if (base.TryGetMember(binder, out result))
                return true;

            string? sResult = null;
            Dictionary<string, string> dicTargetLabel = string.Compare(CurrentCulture.Name, "ko-KR", true) == 0 ? _dicLabelKor : _dicLabelEng;

            if (dicTargetLabel.TryGetValue(binder.Name, out sResult))
            {
                result = sResult;
                return true;
            }

            result = null;
            return false;
        }

        private void initLabels()
        {
            _dicLabelEng.Add(StartWorkTime, StartWorkTime);
            _dicLabelEng.Add(EndWorkTime, EndWorkTime);
            _dicLabelEng.Add(WorkTimeSpan, WorkTimeSpan);
            _dicLabelEng.Add(Year, Year);
            _dicLabelEng.Add(Month, Month);
            _dicLabelEng.Add(Day, Day);
            _dicLabelEng.Add(Hour, Hour);
            _dicLabelEng.Add(Minute, Minute);
            _dicLabelEng.Add(Week, Week);

            _dicLabelKor.Add(StartWorkTime, "근무시작시간");
            _dicLabelKor.Add(EndWorkTime, "근무종료시간");
            _dicLabelKor.Add(WorkTimeSpan, "근무시간");
            _dicLabelKor.Add(Year, "년");
            _dicLabelKor.Add(Month, "월");
            _dicLabelKor.Add(Day, "일");
            _dicLabelKor.Add(Hour, "시");
            _dicLabelKor.Add(Minute, "분");
            _dicLabelKor.Add(Week, "요일");
        }

        private void onCurrentCultureChanged()
        {
            foreach (var key in _dicLabelEng.Keys)
                onPropertyChanged(key);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        private void onPropertyChanged(string propertyName) => PropertyChanged!.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
