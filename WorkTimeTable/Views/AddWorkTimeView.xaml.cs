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

namespace WorkTimeTable.Views
{
    // https://stackoverflow.com/questions/43553243/how-do-you-get-wpf-validation-to-bubble-up-to-a-parent-control
    public partial class AddWorkTimeView : SosoMessageBoxViewBase
    {
        int _errorCount = 0;

        public bool NoErrors
        {
            get => (bool)GetValue(NoErrorsProperty);
            private set => SetValue(NoErrorsPropertyKey, value);
        }

        public AddWorkTimeView()
        {
            InitializeComponent();
            Validation.AddErrorHandler(this, onValidationStack);
        }

        private void onValidationStack(object? sender, ValidationErrorEventArgs e)
        {
            if(e.Action == ValidationErrorEventAction.Added)
            {
                ++_errorCount;
                NoErrors = false;
            }   
            else
            {
                --_errorCount;

                if(_errorCount == 0)
                    NoErrors = true;
            }
        }

        private static readonly DependencyPropertyKey NoErrorsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NoErrors), typeof(bool), typeof(AddWorkTimeView), new UIPropertyMetadata(true));
        public static readonly DependencyProperty NoErrorsProperty = NoErrorsPropertyKey.DependencyProperty;
    }
}
