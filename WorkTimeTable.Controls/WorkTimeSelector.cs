using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WorkTimeTable.Controls
{
    public class WorkTimeSelector : ContentControl
    {
        DateTime _SelectedTime;

        public DateTime SelectedTime => _SelectedTime;
        public int Year
        {
            get => (int)GetValue(YearProperty);
            set => SetValue(YearProperty, value);
        }
        public int Month
        {
            get => (int)GetValue(MonthProperty);
            set => SetValue(MonthProperty, value);
        }
        public int Day
        {
            get => (int)GetValue(DayProperty);
            set => SetValue(DayProperty, value);
        }
        public int Hour
        {
            get => (int)GetValue(HourProperty);
            set => SetValue(HourProperty, value);
        }
        public int Minute
        {
            get => (int)GetValue(MinuteProperty);
            set => SetValue(MinuteProperty, value);
        }

        static WorkTimeSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WorkTimeSelector), new FrameworkPropertyMetadata(typeof(WorkTimeSelector)));
        }
        public WorkTimeSelector()
        {
            _SelectedTime = DateTime.Now;
        }




        public static readonly DependencyProperty YearProperty = DependencyProperty.Register(
            "Year", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(DateTime.Now.Year, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty MonthProperty = DependencyProperty.Register(
            "Month", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(DateTime.Now.Month, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty DayProperty = DependencyProperty.Register(
            "Day", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(DateTime.Now.Day, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HourProperty = DependencyProperty.Register(
            "Hour", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register(
            "Minute", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
