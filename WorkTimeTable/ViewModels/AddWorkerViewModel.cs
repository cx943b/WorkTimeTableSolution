using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SosoThemeLibrary;
using SosoThemeLibrary.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;

namespace WorkTimeTable.ViewModels
{
    public class BirthDateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string? birthDate = value as string;
            if (String.IsNullOrEmpty(birthDate))
                return new ValidationResult("Empty BirthDate");

            if(birthDate.Length != 6)
                return new ValidationResult("BirthDate must be 6 characters (ex. 860811)");

            return ValidationResult.Success;
        }
    }

    internal partial class AddWorkerViewModel : SosoMessageBoxViewModelBase
    {
        readonly ILogger _logger;
        readonly IWorkerManageService _workerMgrSvc;

        [ObservableProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty Name")]
        string _Name;

        [ObservableProperty]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Empty BirthDate")]
        [CustomValidation(typeof(BirthDateValidator), "IsValid", ErrorMessage = "Invalid BirthDate")]
        string _BirthDate;

        [ObservableProperty]
        DayOfWeekFlag _FixedWorkWeeks;

        [ObservableProperty]
        WellknownColor _WellknownColor;

        static AddWorkerViewModel()
        {
            typeof(Colors).GetProperties( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        }


        public AddWorkerViewModel(ILogger<AddWorkerViewModel> logger, IWorkerManageService workerMgrSvc)
        {
            _logger = logger;
            _workerMgrSvc = workerMgrSvc;
        }

        protected override void OnClosing(SosoMessageCloseEventArgs e)
        {
            base.OnClosing(e);

            if(Result == System.Windows.MessageBoxResult.OK)
            {
                ValidateAllProperties();

                if(HasErrors)
                {
                    e.ContinueClose = false;
                }
                else
                {
                    _workerMgrSvc.AddWorker(Name, BirthDate, new SolidColorBrush(WellknownColor.Color), FixedWorkWeeks);
                }
            }
        }

        [RelayCommand]
        private void RequestAddWorker()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                var errors = GetErrors();
                return;
            }



            bool isExistWorker = _workerMgrSvc.IsExistWorker(Name, BirthDate);
            if (isExistWorker)
            {
                MessageBox.Show($"{Name} is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            
        }


    }
}
