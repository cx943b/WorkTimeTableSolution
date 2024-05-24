using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Controls
{
    // ItemSource Type: ObservableCollection<WorkTime>
    public class FilteredWorkTimesBar : Control
    {
        
        public static readonly DependencyProperty ColorNameProperty = DependencyProperty.Register("ColorName", typeof(string), typeof(FilteredWorkTimesBar), new UIPropertyMetadata(nameof(Colors.CornflowerBlue)));
        public static readonly DependencyProperty WorkTimesProperty = DependencyProperty.Register("WorkTimes", typeof(ICollection), typeof(FilteredWorkTimesBar), new UIPropertyMetadata(null));

        public static readonly DependencyProperty BarStartTimeProperty = DependencyProperty.Register("BarStartTime", typeof(DateTime), typeof(FilteredWorkTimesBar),
            new FrameworkPropertyMetadata(DateTime.MinValue));
        public static readonly DependencyProperty BarEndTimeProperty = DependencyProperty.Register("BarEndTime", typeof(DateTime), typeof(FilteredWorkTimesBar),
            new FrameworkPropertyMetadata(DateTime.MinValue));

        public string ColorName
        {
            get => (string)GetValue(ColorNameProperty);
            set => SetValue(ColorNameProperty, value);
        }
        public ICollection WorkTimes
        {
            get => (ICollection)GetValue(WorkTimesProperty);
            set => SetValue(WorkTimesProperty, value);
        }
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

        static FilteredWorkTimesBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(FilteredWorkTimesBar), new FrameworkPropertyMetadata(typeof(FilteredWorkTimesBar)));
    }
}
