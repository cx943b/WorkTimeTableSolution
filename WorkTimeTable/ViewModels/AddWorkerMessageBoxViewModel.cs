using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SosoThemeLibrary;
using System.Windows.Media;
using System.Xml.Linq;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;

namespace WorkTimeTable.ViewModels
{
    public partial class AddWorkerMessageBoxViewModel : SosoMessageBoxViewModelBase
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
        string _ColorName = nameof(Colors.CornflowerBlue);

        public WorkerModel? NewWorker { get; private set; } = null;

        public AddWorkerMessageBoxViewModel(ILogger<AddWorkerMessageBoxViewModel> logger, IWorkerManageService workerMgrSvc)
        {
            _logger = logger;
            _workerMgrSvc = workerMgrSvc;
        }

        protected override void OnClosing(SosoMessageCloseEventArgs e)
        {
            base.OnClosing(e);

            if (MessageResult == System.Windows.MessageBoxResult.OK)
            {
                bool isAdded = _workerMgrSvc.TryAddWorker(Name, BirthDate, ColorName, FixedWorkWeeks, out WorkerModel? newWorker);
                if (isAdded)
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
