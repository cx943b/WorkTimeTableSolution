using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using SosoThemeLibrary;
using System.Windows.Media;
using System.Xml.Linq;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;

namespace WorkTimeTable.ViewModels
{
    public partial class AddWorkerMessageBoxViewModel : SosoMessageBoxViewModelBase<IWorker>
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
                int nextWorkerId = _workerMgrSvc.NextNewWorkerId();
                Result = new WorkerModel() { Id = nextWorkerId, Name = Name, BirthDate = BirthDate, ColorName = this.ColorName, FixedWorkWeeks = this.FixedWorkWeeks };
            }
        }
    }
}
