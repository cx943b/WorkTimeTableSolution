using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WorkTimeTable.Controls
{
    public class EntireWorkTimeBarItem : ItemsControl
    {
        public static readonly DependencyProperty BarStartTimeProperty = WorkTimeBarItem.BarStartTimeProperty.AddOwner(typeof(EntireWorkTimeBarItem), new FrameworkPropertyMetadata(DateTime.MinValue));
        public static readonly DependencyProperty BarEndTimeProperty = WorkTimeBarItem.BarEndTimeProperty.AddOwner(typeof(EntireWorkTimeBarItem), new FrameworkPropertyMetadata(DateTime.MaxValue));

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

        static EntireWorkTimeBarItem() => DefaultStyleKeyProperty.OverrideMetadata(typeof(EntireWorkTimeBarItem), new FrameworkPropertyMetadata(typeof(EntireWorkTimeBarItem)));
    }
}
