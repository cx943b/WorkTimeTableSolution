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
        public const string Add = "Add";
        public const string Cancel = "Cancel";
        public const string Day = "Day";
        public const string DayOfWeek = "DayOfWeek";
        public const string EndWorkTime = "EndWorkTime";
        public const string Filter = "Filter";
        public const string Hour = "Hour";
        public const string Index = "Index";
        public const string Minute = "Minute";
        public const string Month = "Month";
        public const string StartWorkTime = "StartWorkTime";
        public const string WorkTimeSpan = "WorkTimeSpan";
        public const string Year = "Year";
        public const string Name = "Name";
        public const string BirthDate = "BirthDate";
        public const string Color = "Color";

        protected override void InitLabels()
        {
            Dictionary<string, string> dicEngLabel = new Dictionary<string, string>
                {
                    { Add, Add },
                    { Cancel, Cancel },
                    { Day, Day },
                    { DayOfWeek, "DoW" },
                    { EndWorkTime, EndWorkTime },
                    { Filter, Filter },
                    { Hour, Hour },
                    { Index, Index },
                    { Minute, Minute },
                    { Month, Month },
                    { StartWorkTime, StartWorkTime },
                    { WorkTimeSpan, WorkTimeSpan },
                    { Year, Year },
                    { Name, Name },
                    { BirthDate, BirthDate },
                    { Color, Color }
                };

            Dictionary<string, string> dicKorLabel = new Dictionary<string, string>
                {
                    { Add, "추가" },
                    { Cancel, "취소" },
                    { Day, "일" },
                    { DayOfWeek, "요일" },
                    { EndWorkTime, "근무종료시간" },
                    { Filter, "필터" },
                    { Hour, "시" },
                    { Index, "번호" },
                    { Minute, "분" },
                    { Month, "월" },
                    { StartWorkTime, "근무시작시간" },
                    { WorkTimeSpan, "근무시간" },
                    { Year, "년" },
                    { Name, "이름" },
                    { BirthDate, "생년월일" },
                    { Color, "색상" }
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
