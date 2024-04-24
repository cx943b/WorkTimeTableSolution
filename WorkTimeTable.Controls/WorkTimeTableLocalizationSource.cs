using Microsoft.Extensions.Logging;
using SosoThemeLibrary.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable.Controls
{
    public sealed class WorkTimeTableLocalizationSource : LocalizationSourceBase
    {
        public const string Index = "Index";
        public const string StartWorkTime = "StartWorkTime";
        public const string EndWorkTime = "EndWorkTime";
        public const string WorkTimeSpan = "WorkTimeSpan";
        public const string Year = "Year";
        public const string Month = "Month";
        public const string Day = "Day";
        public const string Hour = "Hour";
        public const string Minute = "Minute";
        public const string DayOfWeek = "DayOfWeek";

        protected override void InitLabels()
        {
            Dictionary<string, string> dicEngLabel = new Dictionary<string, string>
            {
                { Index, Index},
                { StartWorkTime, StartWorkTime },
                { EndWorkTime, EndWorkTime },
                { WorkTimeSpan, WorkTimeSpan },
                { Year, Year },
                { Month, Month },
                { Day, Day },
                { Hour, Hour },
                { Minute, Minute },
                { DayOfWeek, "DoW" }
            };

            Dictionary<string, string> dicKorLabel = new Dictionary<string, string>
            {
                { Index, "번호" },
                { StartWorkTime, "근무시작시간" },
                { EndWorkTime, "근무종료시간" },
                { WorkTimeSpan, "근무시간" },
                { Year, "년" },
                { Month, "월" },
                { Day, "일" },
                { Hour, "시" },
                { Minute, "분" },
                { DayOfWeek, "요일" }
            };

            base.AddLabels((new CultureInfo("en-US")).LCID, dicEngLabel);
            base.AddLabels((new CultureInfo("ko-KR")).LCID, dicKorLabel);
        }
        protected override bool ValidLabels()
        {
            var labelNames = typeof(WorkTimeTableLocalizationSource).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Select(fi => fi.Name);
            return base.ValidLabels(labelNames);
        }
    }
}
