using CommunityToolkit.Mvvm.DependencyInjection;
using SosoThemeLibrary;
using SosoThemeLibrary.Controls;
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
    /// AddWorkerView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AddWorkerView : SosoMessageBoxViewBase
    {
        public AddWorkerView()
        {
            InitializeComponent();

            Validation.AddErrorHandler(txtName, onValidationError);
            Validation.AddErrorHandler(txtBirthDate, onValidationError);
        }

        protected override void OnCloseRequest(MessageBoxResult result)
        {
            Validation.RemoveErrorHandler(txtName, onValidationError);
            Validation.RemoveErrorHandler(txtBirthDate, onValidationError);

            base.OnCloseRequest(result);
        }



        private void onValidationError(object? sender, ValidationErrorEventArgs e)
        {
            btnOk.IsEnabled = !Validation.GetHasError(txtName) && !Validation.GetHasError(txtBirthDate);
        }
    }
}
