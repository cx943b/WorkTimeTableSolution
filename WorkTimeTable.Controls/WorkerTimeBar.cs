using System.Windows;
using System.Windows.Controls;

namespace WorkTimeTable.Controls
{
    public class WorkerTimeBar : Control
    {
        public static readonly DependencyProperty BarStartTimeProperty = WorkTimeBar.BarStartTimeProperty.AddOwner(typeof(WorkerTimeBar), new FrameworkPropertyMetadata(DateTime.MinValue));
        public static readonly DependencyProperty BarEndTimeProperty = WorkTimeBar.BarEndTimeProperty.AddOwner(typeof(WorkerTimeBar), new FrameworkPropertyMetadata(DateTime.MaxValue));

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

        static WorkerTimeBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkerTimeBar), new FrameworkPropertyMetadata(typeof(WorkerTimeBar)));
    }
}
