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
        readonly ILogger _logger;
        readonly ObservableCollection<IWorker> _workers = new ObservableCollection<IWorker>();

        public IReadOnlyCollection<IWorker> Workers => _workers;

        public EntireWorkerTimeViewModel(ILogger<EntireWorkerTimeViewModel> logger)
        {
            _logger = logger;

            WeakReferenceMessenger.Default.Register<WorkerListChangedMessage>(this, onWorkerListChanged);
            WeakReferenceMessenger.Default.Register<WorkerListLoadedMessage>(this, onWorkerListLoaded);
        }
        
        public void Dispose()
        {
            WeakReferenceMessenger.Default.Unregister<WorkerListChangedMessage>(this);
            WeakReferenceMessenger.Default.Unregister<WorkerListLoadedMessage>(this);

            _logger.LogInformation($"{nameof(EntireWorkerTimeViewModel)} Disposed");
        }

        private void onWorkerListChanged(object sender, WorkerListChangedMessage message)
        {
            WorkerListChangedMessageArgs args = message.Value;

            if (args.Status == WorkerListChangedStatus.Added)
            {
                foreach(var newWorker in args.Workers)
                {
                    _workers.Add(newWorker);
                    _logger.LogInformation($"{newWorker} Added");
                }
                    
            }
            else if (args.Status == WorkerListChangedStatus.Removed)
            {
                foreach (var oldWorker in args.Workers)
                {
                    if(_workers.Remove(oldWorker))
                    {
                        _logger.LogInformation($"{oldWorker} Removed");
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to remove worker - {oldWorker}");
                    }
                }
                    
            }
        }
        private void onWorkerListLoaded(object sender, WorkerListLoadedMessage message)
        {
            _workers.Clear();

            WorkerListLoadedMessageArgs args = message.Value;
            foreach (var worker in args.Workers)
                _workers.Add(worker);
        }
    }
}
