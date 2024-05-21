using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WorkTimeTable.Controls
{
    public class EntireWorkTimeBar : Control
    {
        public static readonly DependencyProperty BarStartTimeProperty = WorkTimeBar.BarStartTimeProperty.AddOwner(typeof(EntireWorkTimeBar), new FrameworkPropertyMetadata(DateTime.MinValue));
        public static readonly DependencyProperty BarEndTimeProperty = WorkTimeBar.BarEndTimeProperty.AddOwner(typeof(EntireWorkTimeBar), new FrameworkPropertyMetadata(DateTime.MaxValue));

        public DateTime BarStartTime
        {
            get => (DateTime)GetValue(BarStartTimeProperty);
            set => SetValue(BarStartTimeProperty, value);
        }
        public DateTime BarEndTime
        {
            get => (DateTime)GetValue(BarEndTimeProperty);
            set => SetValue(BarEndTimeProperty, value);
        }

        static EntireWorkTimeBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(EntireWorkTimeBar), new FrameworkPropertyMetadata(typeof(EntireWorkTimeBar)));
    }
}
