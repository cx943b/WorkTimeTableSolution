using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;

namespace WorkTimeTable.ViewModels
{
    public partial class AddWorkTimeViewModel : SosoMessageBoxViewModelBase
    {
        readonly ILogger _logger;
        readonly IWorkerManageService _workerMgrSvc;

        [ObservableProperty]
        WorkerModel? _TargetWorker;

        [ObservableProperty]
        int _StartTimeYear = DateTime.Now.Year;
        [ObservableProperty]
        int _StartTimeMonth = DateTime.Now.Month;
        [ObservableProperty]
        int _StartTimeDay = DateTime.Now.Day;

        public AddWorkTimeViewModel(ILogger<AddWorkTimeViewModel> logger, IWorkerManageService workerMgrSvc)
        {
            _logger = logger;
            _workerMgrSvc = workerMgrSvc;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if(TargetWorker == null)
            {
                _logger.LogError($"NullRef: {nameof(TargetWorker)}");
                return;
            }
        }

        public bool SetWorkerId(int workerId)
        {
            if(_workerMgrSvc.LastLoadedWorkers == null)
            {
                _logger.LogError($"NotLoaded: {workerId}");
                return false;
            }

            WorkerModel? targetWorker = _workerMgrSvc.LastLoadedWorkers.FirstOrDefault(x => x.Id == workerId) as WorkerModel;
            if(targetWorker == null)
            {
                _logger.LogError($"NotFound: {workerId}"); ;
                return false;
            }

            TargetWorker = targetWorker;
            return true;
        }
    }
}
