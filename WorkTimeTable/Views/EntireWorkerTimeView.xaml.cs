using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkTimeTable.Controls;
using WorkTimeTable.ViewModels;

namespace WorkTimeTable.Views
{
    /// <summary>
    /// EntireWorkerTimeView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EntireWorkerTimeView : UserControl
    {
        public static readonly DependencyProperty BarStartTimeProperty = WorkTimeBar.BarStartTimeProperty.AddOwner(typeof(EntireWorkerTimeView), new FrameworkPropertyMetadata(DateTime.MinValue));
        public static readonly DependencyProperty BarEndTimeProperty = WorkTimeBar.BarEndTimeProperty.AddOwner(typeof(EntireWorkerTimeView), new FrameworkPropertyMetadata(DateTime.MaxValue));


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

        public EntireWorkerTimeView()
        {
            InitializeComponent();
            
            bool isDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
            if(!isDesignMode)
                DataContext = Ioc.Default.GetService<EntireWorkTimeViewModel>();
        }
    }
}
