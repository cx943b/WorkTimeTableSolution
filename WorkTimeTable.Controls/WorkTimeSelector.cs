using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SosoThemeLibrary.Extensions;
using WorkTimeTable.Infrastructure.Validations;

namespace WorkTimeTable.Controls
{
    [TemplatePart(Name = PART_TextBox_Year, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_TextBox_Month, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_TextBox_Day, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_TextBox_Hour, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_TextBox_Minute, Type = typeof(TextBox))]
    public class WorkTimeSelector : ContentControl
    {
        const string PART_TextBox_Year = "PART_TextBox_Year";
        const string PART_TextBox_Month = "PART_TextBox_Month";
        const string PART_TextBox_Day = "PART_TextBox_Day";
        const string PART_TextBox_Hour = "PART_TextBox_Hour";
        const string PART_TextBox_Minute = "PART_TextBox_Minute";

        ChangedBy _YearChangedBy, _MonthChangedBy, _DayChangedBy = ChangedBy.None;
        ChangedBy _HourChangedBy, _MinuteChangedBy = ChangedBy.None;

        TextBox? _txtYear, _txtMonth, _txtDay;
        TextBox? _txtHour, _txtMinute;

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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _txtYear = GetTemplateChild(PART_TextBox_Year) as TextBox;
            if(_txtYear == null)
                throw new NullReferenceException(nameof(_txtYear));

            _txtMonth = GetTemplateChild(PART_TextBox_Month) as TextBox;
            if (_txtMonth == null)
                throw new NullReferenceException(nameof(_txtMonth));

            _txtDay = GetTemplateChild(PART_TextBox_Day) as TextBox;
            if (_txtDay == null)
                throw new NullReferenceException(nameof(_txtDay));

            _txtHour = GetTemplateChild(PART_TextBox_Hour) as TextBox;
            if (_txtHour == null)
                throw new NullReferenceException(nameof(_txtHour));

            _txtMinute = GetTemplateChild(PART_TextBox_Minute) as TextBox;
            if (_txtMinute == null)
                throw new NullReferenceException(nameof(_txtMinute));

            _txtYear.TextChanged += onYearTextChanged;
            _txtMonth.TextChanged += onMonthTextChanged;
            _txtDay.TextChanged += onDayTextChanged;
            _txtHour.TextChanged += onHourTextChanged;
            _txtMinute.TextChanged += onMinuteTextChanged;
        }

        private static void onYearTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? txtYear = sender as TextBox;
            if (txtYear == null)
                throw new NullReferenceException(nameof(txtYear));

            var workTimeSelector = txtYear.FindVisualParent<WorkTimeSelector>();
            if(workTimeSelector == null)
                throw new NullReferenceException(nameof(workTimeSelector));

            if(workTimeSelector._YearChangedBy == ChangedBy.None)
            {
                workTimeSelector._YearChangedBy = ChangedBy.TextBox;
                workTimeSelector.Year = int.Parse(txtYear.Text);
            }
            else
            {
                workTimeSelector._YearChangedBy = ChangedBy.None;
            }
        }
        private static void onMonthTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? txtMonth = sender as TextBox;
            if (txtMonth == null)
                throw new NullReferenceException(nameof(txtMonth));
                
            var workTimeSelector = txtMonth.FindVisualParent<WorkTimeSelector>();
            if (workTimeSelector == null)
                throw new NullReferenceException(nameof(workTimeSelector));
            
            if (workTimeSelector._MonthChangedBy == ChangedBy.None)
            {
                workTimeSelector._MonthChangedBy = ChangedBy.TextBox;
                workTimeSelector.Month = int.Parse(txtMonth.Text);
            }
            else
            {
                workTimeSelector._MonthChangedBy = ChangedBy.None;
            }
        }
        private static void onDayTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? txtDay = sender as TextBox;
            if (txtDay == null)
                throw new NullReferenceException(nameof(txtDay));
                
            var workTimeSelector = txtDay.FindVisualParent<WorkTimeSelector>();
            if (workTimeSelector == null)
                throw new NullReferenceException(nameof(workTimeSelector));
            
            if (workTimeSelector._DayChangedBy == ChangedBy.None)
            {
                workTimeSelector._DayChangedBy = ChangedBy.TextBox;
                workTimeSelector.Day = int.Parse(txtDay.Text);
            }
            else
            {
                workTimeSelector._DayChangedBy = ChangedBy.None;
            }
        }
        private static void onHourTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? txtHour = sender as TextBox;
            if (txtHour == null)
                throw new NullReferenceException(nameof(txtHour));
                
            var workTimeSelector = txtHour.FindVisualParent<WorkTimeSelector>();
            if (workTimeSelector == null)
                throw new NullReferenceException(nameof(workTimeSelector));
            
            if (workTimeSelector._HourChangedBy == ChangedBy.None)
            {
                workTimeSelector._HourChangedBy = ChangedBy.TextBox;
                workTimeSelector.Hour = int.Parse(txtHour.Text);
            }
            else
            {
                workTimeSelector._HourChangedBy = ChangedBy.None;
            }
        }
        private static void onMinuteTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox? txtMinute = sender as TextBox;
            if (txtMinute == null)
                throw new NullReferenceException(nameof(txtMinute));
                
            var workTimeSelector = txtMinute.FindVisualParent<WorkTimeSelector>();
            if (workTimeSelector == null)
                throw new NullReferenceException(nameof(workTimeSelector));
            
            if (workTimeSelector._MinuteChangedBy == ChangedBy.None)
            {
                workTimeSelector._MinuteChangedBy = ChangedBy.TextBox;
                workTimeSelector.Minute = int.Parse(txtMinute.Text);
            }
            else
            {
                workTimeSelector._MinuteChangedBy = ChangedBy.None;
            }
        }

        private static void onYearPropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            WorkTimeSelector? workTimeSelector = depObj as WorkTimeSelector;
            if(workTimeSelector == null)
                throw new NullReferenceException(nameof(workTimeSelector));
            
            if(workTimeSelector._YearChangedBy == ChangedBy.None)
            {
                workTimeSelector._txtYear!.Text = e.NewValue.ToString();
            }
            else
            {
                workTimeSelector._YearChangedBy = ChangedBy.None;
            }

            BindingExpression bindExpr = BindingOperations.GetBindingExpression(workTimeSelector, WorkTimeSelector.YearProperty);
            Validation.MarkInvalid(bindExpr, new ValidationError(new YearValidationRule(), bindExpr));
        }
        private static void onMonthPropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            WorkTimeSelector? workTimeSelector = depObj as WorkTimeSelector;
            if(workTimeSelector == null)
                throw new NullReferenceException(nameof(workTimeSelector));
            
            if(workTimeSelector._MonthChangedBy == ChangedBy.None)
            {
                workTimeSelector._txtMonth!.Text = e.NewValue.ToString();
            }
            else
            {
                workTimeSelector._MonthChangedBy = ChangedBy.None;
            }

            BindingExpression bindExpr = BindingOperations.GetBindingExpression(workTimeSelector, WorkTimeSelector.MonthProperty);
            Validation.MarkInvalid(bindExpr, new ValidationError(new MonthValidationRule(), bindExpr));
        }
        private static void onDayPropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            WorkTimeSelector? workTimeSelector = depObj as WorkTimeSelector;
            if(workTimeSelector == null)
                throw new NullReferenceException(nameof(workTimeSelector));
            
            if(workTimeSelector._DayChangedBy == ChangedBy.None)
            {
                workTimeSelector._txtDay!.Text = e.NewValue.ToString();
            }
            else
            {
                workTimeSelector._DayChangedBy = ChangedBy.None;
            }

            BindingExpression bindExpr = BindingOperations.GetBindingExpression(workTimeSelector, WorkTimeSelector.DayProperty);
            Validation.MarkInvalid(bindExpr, new ValidationError(new DayValidationRule(), bindExpr));
        }
        private static void onHourPropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {

        }
        private static void onMinutePropertyChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {

        }



        public static readonly DependencyProperty YearProperty = DependencyProperty.Register(
            "Year", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(DateTime.MinValue.Year, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, onYearPropertyChanged));
        public static readonly DependencyProperty MonthProperty = DependencyProperty.Register(
            "Month", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(DateTime.MinValue.Month, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty DayProperty = DependencyProperty.Register(
            "Day", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(DateTime.MinValue.Day, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty HourProperty = DependencyProperty.Register(
            "Hour", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty MinuteProperty = DependencyProperty.Register(
            "Minute", typeof(int), typeof(WorkTimeSelector), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        enum ChangedBy { None, TextBox, DependencyProperty };
    }
}
