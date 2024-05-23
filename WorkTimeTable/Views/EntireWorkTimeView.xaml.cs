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
using WorkTimeTable.Infrastructure;

namespace WorkTimeTable.Views
{
    public partial class EntireWorkTimeView : UserControl
    {
        public EntireWorkTimeView()
        {
            InitializeComponent();
            
            bool isDesignMode = System.ComponentModel.DesignerProperties.GetIsInDesignMode(this);
            if(!isDesignMode)
            {
                DataContext = Ioc.Default.GetServiceForViewModel(nameof(EntireWorkTimeView));
            }
        }
    }
}
