using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable.Services
{
    public class LocalizationService : DynamicObject, INotifyPropertyChanged
    {
        public const string StartWorkTime = "StartWorkTime";
        public const string EndWorkTime = "EndWorkTime";
        public const string WorkTimeSpan = "WorkTimeSpan";

        readonly Dictionary<string, string> _dicLabelKor = new Dictionary<string, string>();
        readonly Dictionary<string, string> _dicLabelEng = new Dictionary<string, string>();

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        public CultureInfo CurrentCulture { get; private set; } = CultureInfo.CurrentCulture;

        public LocalizationService()
        {
            initLabels();
        }

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            if (value == null || value is not string)
                return false;

            string key = binder.Name;
            string sValue = (string)value;
            Dictionary<string, string> dicTargetLabel = (String.Compare(CurrentCulture.Name, "ko-KR", true) == 0) ? _dicLabelKor : _dicLabelEng;

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
            Dictionary<string, string> dicTargetLabel = (String.Compare(CurrentCulture.Name, "ko-KR", true) == 0) ? _dicLabelKor : _dicLabelEng;
          
            if (dicTargetLabel.TryGetValue(binder.Name, out sResult))
            {
                result = sResult;
                return true;
            }

            result = null;
            return false;
        }

        public void ChangeCulture(CultureInfo targetCulture)
        {
            if(targetCulture == null)
                throw new ArgumentNullException(nameof(targetCulture));

            if(targetCulture != CurrentCulture)
            {
                CurrentCulture = targetCulture;
                resetLabels();
            }
        }

        private void resetLabels()
        {
            foreach (var key in _dicLabelEng.Keys)
                onPropertyChanged(key);
        }
        private void initLabels()
        {
            _dicLabelEng.Add(StartWorkTime, StartWorkTime);
            _dicLabelEng.Add(EndWorkTime, EndWorkTime);
            _dicLabelEng.Add(WorkTimeSpan, WorkTimeSpan);

            _dicLabelKor.Add(StartWorkTime, "근무시작시간");
            _dicLabelKor.Add(EndWorkTime, "근무종료시간");
            _dicLabelKor.Add(WorkTimeSpan, "근무시간");
        }

        private void onPropertyChanged(string propertyName) => PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
    }
}
