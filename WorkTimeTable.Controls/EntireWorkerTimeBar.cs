using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WorkTimeTable.Controls
{
    public class EntireWorkerTimeBar : ItemsControl
    {
        public static readonly DependencyProperty BarStartTimeProperty = WorkTimeBar.BarStartTimeProperty.AddOwner(typeof(EntireWorkerTimeBar), new FrameworkPropertyMetadata(DateTime.MinValue));
        public static readonly DependencyProperty BarEndTimeProperty = WorkTimeBar.BarEndTimeProperty.AddOwner(typeof(EntireWorkerTimeBar), new FrameworkPropertyMetadata(DateTime.MaxValue));

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

        static EntireWorkerTimeBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(EntireWorkerTimeBar), new FrameworkPropertyMetadata(typeof(EntireWorkerTimeBar)));
    }
}
