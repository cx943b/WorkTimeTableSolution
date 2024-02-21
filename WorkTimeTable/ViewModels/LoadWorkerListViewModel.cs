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

namespace WorkTimeTable.ViewModels
{
    internal partial class LoadWorkerListViewModel
    {
        readonly ILogger _logger;
        readonly IWorkerManageService _workerMgrSvc;

        public LoadWorkerListViewModel(ILogger<LoadWorkerListViewModel> logger, IWorkerManageService workerMgrSvc)
        {
            _logger = logger;
            _workerMgrSvc = workerMgrSvc;
        }

        [RelayCommand]
        private async Task LoadWorkerListAsync() => await _workerMgrSvc.LoadWorkersAsync();
    }
}
