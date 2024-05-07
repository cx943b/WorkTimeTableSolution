using System.Windows;
using System.Windows.Controls;

namespace WorkTimeTable.Controls
{
    public class WorkTimeBar : ItemsControl
    {
        public static readonly DependencyProperty BarStartTimeProperty = WorkTimeBarItem.BarStartTimeProperty.AddOwner(typeof(WorkTimeBar), new FrameworkPropertyMetadata(DateTime.MinValue));
        public static readonly DependencyProperty BarEndTimeProperty = WorkTimeBarItem.BarEndTimeProperty.AddOwner(typeof(WorkTimeBar), new FrameworkPropertyMetadata(DateTime.MaxValue));

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

        static WorkTimeBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkTimeBar), new FrameworkPropertyMetadata(typeof(WorkTimeBar)));
    }
}
