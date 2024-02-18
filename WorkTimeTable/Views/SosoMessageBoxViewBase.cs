using SosoThemeLibrary;
using SosoThemeLibrary.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WorkTimeTable.Views
{
    public class SosoMessageBoxViewBase : SosoMessageBoxContentControl
    {
        protected override void OnOKButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ISosoMessageBoxViewModel;
            if (vm != null)
                vm.MessageResult = MessageBoxResult.OK;

            base.OnOKButtonClick(sender, e);
        }
        protected override void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ISosoMessageBoxViewModel;
            if (vm != null)
                vm.MessageResult = MessageBoxResult.Cancel;

            base.OnCancelButtonClick(sender, e);
        }
    }
}
