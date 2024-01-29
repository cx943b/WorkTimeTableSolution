using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    internal partial class AddWorkerViewModel : ObservableValidator
    {
        static readonly Color _WorkerColors;

        readonly ILogger _logger;
        readonly IWorkerManageService _workerMgrSvc;

        [ObservableProperty]
        [Required]
        string _Name;

        [ObservableProperty]
        [Required]
        [RegularExpression("[0-9]{6}")]
        string _BirthDate;

        [ObservableProperty]
        DayOfWeekFlag _FixedWorkWeeks;

        static AddWorkerViewModel()
        {
            typeof(Colors).GetProperties( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        }


        public AddWorkerViewModel(ILogger<AddWorkerViewModel> logger, IWorkerManageService workerMgrSvc)
        {
            _logger = logger;
            _workerMgrSvc = workerMgrSvc;
        }

        [RelayCommand]
        private void RequestAddWorker()
        {
            bool isExistWorker = _workerMgrSvc.IsExistWorker(Name, BirthDate);
            if (isExistWorker)
            {
                MessageBox.Show($"{Name} is already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }
    }
}
