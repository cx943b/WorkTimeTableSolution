using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Messages;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;
using WorkTimeTable.Views;

namespace WorkTimeTable.ViewModels
{
    internal partial class MainViewModel : ObservableObject
    {
        private readonly ILogger _logger;
        private readonly IWorkerManageService _workerMgrSvc;

        [ObservableProperty]
        WorkerModel _Worker;

        public MainViewModel(ILogger<MainViewModel> logger, IWorkerManageService workerMgrSvc)
        {
            _logger = logger;
            _workerMgrSvc = workerMgrSvc;

            //Worker = _workerMgrSvc.LastLoadedWorkers.Cast<WorkerModel>().First();

            WeakReferenceMessenger.Default.Register<WorkerListLoadedMessage>(this, onWorkerListChanged);
        }

        private void onWorkerListChanged(object sender, WorkerListLoadedMessage message)
        {
            Worker = message.Value.Workers.Cast<WorkerModel>().First();
        }
    }
}
