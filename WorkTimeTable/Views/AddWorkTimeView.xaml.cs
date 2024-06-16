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


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Part_Ok_Button.Click += base.OnOKButtonClick;
            Part_Cancel_Button.Click += base.OnCancelButtonClick;
        }

        protected override void OnCloseConformed()
        {
            base.OnCloseConformed();
            Validation.RemoveErrorHandler(this, onValidationStack);
        }

        private static void onValidationStack(object? sender, ValidationErrorEventArgs e)
        {
            AddWorkTimeView? view = sender as AddWorkTimeView;
            if (view == null)
                return;

            if (e.Action == ValidationErrorEventAction.Added)
            {
                ++view._errorCount;
                view.NoErrors = false;
            }   
            else
            {
                --view._errorCount;

                if(view._errorCount == 0)
                    view.NoErrors = true;
            }
        }

        private static readonly DependencyPropertyKey NoErrorsPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NoErrors), typeof(bool), typeof(AddWorkTimeView), new UIPropertyMetadata(true));
        public static readonly DependencyProperty NoErrorsProperty = NoErrorsPropertyKey.DependencyProperty;
    }
}
