using System;
using System.Collections;
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
        public static readonly DependencyProperty FilteredWorkTimesListProperty = DependencyProperty.Register("FilteredWorkTimesList", typeof(IEnumerable), typeof(EntireWorkTimeBar),
            new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty BarStartTimeProperty = DependencyProperty.Register("BarStartTime", typeof(DateTime), typeof(EntireWorkTimeBar),
            new FrameworkPropertyMetadata(DateTime.MinValue));
        public static readonly DependencyProperty BarEndTimeProperty = DependencyProperty.Register("BarEndTime", typeof(DateTime), typeof(EntireWorkTimeBar),
            new FrameworkPropertyMetadata(DateTime.MinValue));

        public IEnumerable FilteredWorkTimesList
        {
            get => (IEnumerable)GetValue(FilteredWorkTimesListProperty);
            set => SetValue(FilteredWorkTimesListProperty, value);
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

        static EntireWorkTimeBar() => DefaultStyleKeyProperty.OverrideMetadata(typeof(EntireWorkTimeBar), new FrameworkPropertyMetadata(typeof(EntireWorkTimeBar)));
    }
}
