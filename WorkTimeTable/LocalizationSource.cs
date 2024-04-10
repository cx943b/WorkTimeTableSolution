using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable
{
    public abstract class LocalizationSourceBase : DynamicObject, INotifyPropertyChanged
    {
        CultureInfo _CurrentCulture = CultureInfo.CurrentCulture;

        // Key: CultureInfo.LCID
        readonly Dictionary<int, IDictionary<string, string>> _dicLabelsByCultureID = new Dictionary<int, IDictionary<string, string>>();

        public CultureInfo CurrentCulture
        {
            get => _CurrentCulture;
            set
            {
                if (value is null)
                    throw new NullReferenceException(nameof(CurrentCulture));

                if (value != _CurrentCulture)
                {
                    _CurrentCulture = value;
                    onCurrentCultureChanged();
                }
            }
        }

        public LocalizationSourceBase() => InitLabels();

        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            if (value == null || value is not string)
                return false;

            string key = binder.Name;
            string sValue = (string)value;
            IDictionary<string, string> dicTargetLabel = _dicLabelsByCultureID[_CurrentCulture.LCID];

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
            IDictionary<string, string> dicTargetLabel = _dicLabelsByCultureID[_CurrentCulture.LCID];

            if (dicTargetLabel.TryGetValue(binder.Name, out sResult))
            {
                result = sResult;
                return true;
            }

            result = null;
            return false;
        }

        protected abstract void InitLabels();
        protected abstract bool ValidLabels();
        
        protected void AddLabels(int cultureID, IDictionary<string, string> dicLabels)
        {
            if (dicLabels is null)
                throw new ArgumentNullException(nameof(dicLabels));

            if (_dicLabelsByCultureID.ContainsKey(cultureID))
                _dicLabelsByCultureID[cultureID] = dicLabels;
            else
                _dicLabelsByCultureID.Add(cultureID, dicLabels);
        }

        private void onCurrentCultureChanged()
        {
            foreach (var key in _dicLabelsByCultureID[_CurrentCulture.LCID].Keys)
                onPropertyChanged(key);
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged = delegate { };
        private void onPropertyChanged(string propertyName) => PropertyChanged!.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }

    public sealed class WorkTimeTableLocalizationSource : LocalizationSourceBase
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

        protected override void InitLabels()
        {
            Dictionary<string, string> dicEngLabel = new Dictionary<string, string>
            {
                {StartWorkTime, StartWorkTime },
                {EndWorkTime, EndWorkTime },
                {WorkTimeSpan, WorkTimeSpan },
                {Year, Year },
                {Month, Month },
                {Day, Day },
                {Hour, Hour },
                {Minute, Minute },
                {Week, Week }
            };

            Dictionary<string, string> dicKorLabel = new Dictionary<string, string>
            {
                {StartWorkTime, "근무시작시간" },
                {EndWorkTime, "근무종료시간" },
                {WorkTimeSpan, "근무시간" },
                {Year, "년" },
                {Month, "월" },
                {Day, "일" },
                {Hour, "시" },
                {Minute, "분" },
                {Week, "요일" }
            };

            base.AddLabels((new CultureInfo("en-US")).LCID, dicEngLabel);
            base.AddLabels((new CultureInfo("ko-KR")).LCID, dicKorLabel);
        }
        protected override bool ValidLabels()
        {
            var labelNames =  typeof(WorkTimeTableLocalizationSource).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Select(fi => fi.Name);
            return true;
        }
    }
}
