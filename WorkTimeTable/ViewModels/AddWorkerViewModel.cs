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
    internal partial class AddWorkerViewModel : AddWorkerMessageBoxViewModel
    {
        readonly ILogger _logger;
        readonly IWorkerManageService _workerMgrSvc;

        [ObservableProperty]
        string _Name;

        [ObservableProperty]
        string _BirthDate;

        [ObservableProperty]
        DayOfWeekFlag _FixedWorkWeeks;

        [ObservableProperty]
        WellknownColor _WellknownColor = new WellknownColor(nameof(Colors.CornflowerBlue), Colors.CornflowerBlue);

        static AddWorkerViewModel()
        {
            typeof(Colors).GetProperties( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        }


        public AddWorkerViewModel(ILogger<AddWorkerViewModel> logger, IWorkerManageService workerMgrSvc)
        {
            _logger = logger;
            _workerMgrSvc = workerMgrSvc;

            WellknownColor = new WellknownColor(nameof(Colors.CornflowerBlue), Colors.CornflowerBlue);
            //Title = "Add New Worker";
        }

        protected override void OnClosing(SosoMessageCloseEventArgs e)
        {
            base.OnClosing(e);

            if(MessageResult == System.Windows.MessageBoxResult.OK)
            {
                bool isAdded = _workerMgrSvc.TryAddWorker(Name, BirthDate, WellknownColor.Name, FixedWorkWeeks, out WorkerModel? newWorker);
                if(isAdded)
                {
                    NewWorker = newWorker;
                }
                else
                {
                    _logger.LogError("Failed: Add new worker");
                }
            }
        }
    }
}
