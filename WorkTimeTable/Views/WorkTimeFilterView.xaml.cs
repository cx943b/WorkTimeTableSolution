using CommunityToolkit.Mvvm.DependencyInjection;
using System;
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
using WorkTimeTable.ViewModels;

namespace WorkTimeTable.Views
{
    /// <summary>
    /// YearMonthFilterView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WorkTimeFilterView : UserControl
    {
        public WorkTimeFilterView()
        {
            InitializeComponent();

            if(!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
                DataContext = Ioc.Default.GetService<WorkTimeFilterViewModel>();
        }
    }
}
