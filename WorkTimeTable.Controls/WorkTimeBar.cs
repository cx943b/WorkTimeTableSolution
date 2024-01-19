using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WorkTimeTable.Controls
{
    public class WorkTimeBar : FrameworkElement
    {
        public static readonly DependencyProperty BarStartTimeProperty = DependencyProperty.Register("BarStartTime", typeof(DateTime), typeof(WorkTimeBar),
            new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.AffectsRender), validateValueCallback: onBarStartTimeValidate);
        public static readonly DependencyProperty BarEndTimeProperty = DependencyProperty.Register("BarEndTime", typeof(DateTime), typeof(WorkTimeBar),
            new FrameworkPropertyMetadata(DateTime.MaxValue, FrameworkPropertyMetadataOptions.AffectsRender), validateValueCallback: onBarEndTimeValidate);

        public static readonly DependencyProperty WorkStartTimeProperty = DependencyProperty.Register("WorkStartTime", typeof(DateTime), typeof(WorkTimeBar),
            new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.AffectsRender));
        public static readonly DependencyProperty WorkTimeSpenProperty = DependencyProperty.Register("WorkTimeSpan", typeof(TimeSpan), typeof(WorkTimeBar),
            new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty WorkBrushProperty = DependencyProperty.Register("WorkBrush", typeof(Brush), typeof(WorkTimeBar),
            new FrameworkPropertyMetadata(Brushes.CornflowerBlue, FrameworkPropertyMetadataOptions.AffectsRender));

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

        public DateTime WorkStartTime
        {
            get => (DateTime)GetValue(WorkStartTimeProperty);
            set => SetValue(WorkStartTimeProperty, value);
        }
        public TimeSpan WorkTimeSpan
        {
            get => (TimeSpan)GetValue(WorkTimeSpenProperty);
            set => SetValue(WorkTimeSpenProperty, value);
        }
        public Brush WorkBrush
        {
            get => (Brush)GetValue(WorkBrushProperty);
            set => SetValue(WorkBrushProperty, value);
        }

        public WorkTimeBar()
        {
            UseLayoutRounding = true;
            SnapsToDevicePixels = true;
        }




        protected override void OnRender(DrawingContext dc)
        {
            TimeSpan workTime = WorkTimeSpan;
            if(workTime == TimeSpan.Zero)
                return;

            DateTime barStartTime = BarStartTime;
            DateTime barEndTime = BarEndTime;

            double totBarMinites = (barEndTime - barStartTime).TotalMinutes;
            double barWidthPerMinute = ActualWidth / totBarMinites;
            double workStartPosX = (WorkStartTime - barStartTime).TotalMinutes * barWidthPerMinute;

            if (workStartPosX < 0)
            {
                workStartPosX = 0;
                workTime = this.WorkTimeSpan - (barStartTime - WorkStartTime);
            }

            double workWidth = workTime.TotalMinutes * barWidthPerMinute;
            if(workStartPosX + workWidth > ActualWidth)
                workWidth = ActualWidth - workStartPosX;

            dc.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));
            dc.DrawRectangle(WorkBrush, null, new Rect(Math.Truncate(workStartPosX), 0, Math.Ceiling(workWidth), ActualHeight));
            dc.Pop();
        }


        private static bool onBarStartTimeValidate(object value) => (DateTime)value >= DateTime.MinValue;
        private static bool onBarEndTimeValidate(object value) => (DateTime)value <= DateTime.MaxValue;
    }
}