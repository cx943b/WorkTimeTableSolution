using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Messages;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;

namespace WorkTimeTable.ViewModels
{
    internal partial class EntireWorkerTimeViewModel : ObservableObject, IDisposable
    {
        readonly IWorkerManageService _workerMgrSvc;
        readonly ILogger _logger;

        readonly ObservableCollection<IWorker> _workers = new ObservableCollection<IWorker>();

        public IReadOnlyCollection<IWorker> Workers => _workers;

        public EntireWorkerTimeViewModel(ILogger<EntireWorkerTimeViewModel> logger, IWorkerManageService workerMgrSvc)
        {
            _logger = logger;
            _workerMgrSvc = workerMgrSvc;

            WeakReferenceMessenger.Default.Register<WorkerListChangedMessage>(this, onWorkerListChanged);
        }

        [RelayCommand()]
        private async Task LoadWorkersAsync()
        {
            _workers.Clear();

            var workers = await _workerMgrSvc.LoadWorkersAsync();
            if(workers is null)
            {
                _logger.LogError("Failed to load workers");
                return;
            }

            _logger.LogInformation($"Loaded {workers.Count()} workers");

            if (workers.Any())
            {
                foreach (var worker in workers)
                    _workers.Add(worker);
            }
        }
        public void Dispose()
        {
            WeakReferenceMessenger.Default.Unregister<WorkerListChangedMessage>(this);
        }

        private void onWorkerListChanged(object sender, WorkerListChangedMessage message)
        {
            WorkerListChangedMessageArgs args = message.Value;

            if (args.Status == WorkerListChangedStatus.Added)
            {
                foreach(var newWorker in args.Workers)
                    _workers.Add(newWorker);
            }
            else if (args.Status == WorkerListChangedStatus.Removed)
            {
                bool isRemoved = false;
                foreach (var oldWorker in args.Workers)
                {
                    isRemoved = _workers.Remove(oldWorker);
                    if(!isRemoved)
                    {
                        _logger.LogWarning($"Failed to remove worker - {oldWorker}");
                    }
                }
                    
            }
        }
    }
}
