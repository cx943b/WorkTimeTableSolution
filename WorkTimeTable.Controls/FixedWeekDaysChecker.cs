using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkTimeTable.Infrastructure;

namespace WorkTimeTable.Controls
{
    public class FixedWeekDaysChecker : Control
    {
        static readonly IReadOnlyCollection<DayOfWeekFlag> _DayOfWeekFlags = Enum.GetValues<DayOfWeekFlag>().ToArray();
        public DayOfWeekFlag FixedWeekDays
        {
            get { return (DayOfWeekFlag)GetValue(FixedWeekDaysProperty); }
            set { SetValue(FixedWeekDaysProperty, value); }
        }
        public IReadOnlyCollection<DayOfWeekFlag> DayOfWeekFlags => (IReadOnlyCollection<DayOfWeekFlag>)GetValue(DayOfWeekFlagsProperty);

        static FixedWeekDaysChecker() => DefaultStyleKeyProperty.OverrideMetadata(typeof(FixedWeekDaysChecker), new FrameworkPropertyMetadata(typeof(FixedWeekDaysChecker)));
        public FixedWeekDaysChecker()
        {
            SetValue(DayOfWeekFlagsPropertyKey, _DayOfWeekFlags);
        }

        

        public static readonly DependencyProperty FixedWeekDaysProperty =
            DependencyProperty.Register("FixedWeekDays", typeof(DayOfWeekFlag), typeof(FixedWeekDaysChecker), new PropertyMetadata(DayOfWeekFlag.None));
        private static readonly DependencyPropertyKey DayOfWeekFlagsPropertyKey = DependencyProperty.RegisterReadOnly("DayOfWeekFlags", typeof(IReadOnlyCollection<DayOfWeekFlag>), typeof(FixedWeekDaysChecker), new PropertyMetadata(null));
        public static readonly DependencyProperty DayOfWeekFlagsProperty = DayOfWeekFlagsPropertyKey.DependencyProperty;

    }
}
