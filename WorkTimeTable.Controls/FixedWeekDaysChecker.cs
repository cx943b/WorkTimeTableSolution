using CommunityToolkit.Mvvm.Input;
using SosoThemeLibrary.Extensions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WorkTimeTable.Infrastructure;

namespace WorkTimeTable.Controls
{
    public class FixedWeekDaysChecker : Control
    {
        static readonly RelayCommand<RoutedEventArgs> _CheckedCommand = new RelayCommand<RoutedEventArgs>(onChecked);
        static readonly RelayCommand<RoutedEventArgs> _UncheckedCommand = new RelayCommand<RoutedEventArgs>(onUnchecked);
        static readonly IReadOnlyCollection<DayOfWeekFlag> _DayOfWeekFlags = Enum.GetValues<DayOfWeekFlag>().Skip(1).ToArray();

        public DayOfWeekFlag FixedWeekDays
        {
            get { return (DayOfWeekFlag)GetValue(FixedWeekDaysProperty); }
            set { SetValue(FixedWeekDaysProperty, value); }
        }
        public IReadOnlyCollection<DayOfWeekFlag> DayOfWeekFlags => (IReadOnlyCollection<DayOfWeekFlag>)GetValue(DayOfWeekFlagsProperty);

        
        internal ICommand CheckedCommand => (ICommand)GetValue(CheckedCommandPropertyKey.DependencyProperty);
        internal ICommand UncheckedCommand => (ICommand)GetValue(UncheckedCommandPropertyKey.DependencyProperty);

        
        public event RoutedEventHandler FixedWeekDaysChanged
        {
            add { AddHandler(FixedWeekDaysChangedEvent, value); }
            remove { RemoveHandler(FixedWeekDaysChangedEvent, value); }
        }

        static FixedWeekDaysChecker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FixedWeekDaysChecker), new FrameworkPropertyMetadata(typeof(FixedWeekDaysChecker)));
        }
        
        public FixedWeekDaysChecker()
        {
            SetValue(DayOfWeekFlagsPropertyKey, _DayOfWeekFlags);
        }

        private void onChecked(DayOfWeekFlag newFlag)
        {
            FixedWeekDays |= newFlag;
            this.RaiseEvent(new RoutedEventArgs(FixedWeekDaysChangedEvent));
        }
        private void onUnchecked(DayOfWeekFlag oldFlag)
        {
            FixedWeekDays &= ~oldFlag;
            this.RaiseEvent(new RoutedEventArgs(FixedWeekDaysChangedEvent));
        }

        private static void onChecked(RoutedEventArgs? e)
        {
            if (!(e.Source is CheckBox chkBox) || !(chkBox.DataContext is DayOfWeekFlag dayOfWeekFlag))
                return;

            chkBox.FindVisualParent<FixedWeekDaysChecker>()?.onChecked(dayOfWeekFlag);
        }
        private static void onUnchecked(RoutedEventArgs? e)
        {
            if (!(e.Source is CheckBox chkBox) || !(chkBox.DataContext is DayOfWeekFlag dayOfWeekFlag))
                return;

            chkBox.FindVisualParent<FixedWeekDaysChecker>()?.onUnchecked(dayOfWeekFlag);
        }



        public static readonly RoutedEvent FixedWeekDaysChangedEvent = EventManager.RegisterRoutedEvent(nameof(FixedWeekDaysChanged), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FixedWeekDaysChecker));

        public static readonly DependencyProperty FixedWeekDaysProperty = DependencyProperty.Register(
            "FixedWeekDays", typeof(DayOfWeekFlag), typeof(FixedWeekDaysChecker), new PropertyMetadata(DayOfWeekFlag.None));

        private static readonly DependencyPropertyKey DayOfWeekFlagsPropertyKey = DependencyProperty.RegisterReadOnly(
            "DayOfWeekFlags", typeof(IReadOnlyCollection<DayOfWeekFlag>), typeof(FixedWeekDaysChecker), new UIPropertyMetadata(_DayOfWeekFlags));

        private static readonly DependencyPropertyKey CheckedCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(CheckedCommand), typeof(ICommand), typeof(FixedWeekDaysChecker), new UIPropertyMetadata(_CheckedCommand));

        private static readonly DependencyPropertyKey UncheckedCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(UncheckedCommand), typeof(ICommand), typeof(FixedWeekDaysChecker), new UIPropertyMetadata(_UncheckedCommand));

        public static readonly DependencyProperty DayOfWeekFlagsProperty = DayOfWeekFlagsPropertyKey.DependencyProperty;
        //public static readonly DependencyProperty CheckedCommandProperty = CheckedCommandPropertyKey.DependencyProperty;
        //public static readonly DependencyProperty UncheckedCommandProperty = UncheckedCommandPropertyKey.DependencyProperty;


    }
}
