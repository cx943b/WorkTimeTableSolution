using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkTimeTable.Controls;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Messages;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;

namespace WorkTimeTable.ViewModels
{
    internal partial class EntireWorkTimeViewModel : ObservableObject, IDisposable
    {
        WorkTimeFilter? _currentFilter;

        readonly ILogger _logger;
        readonly IList<IWorker> _workers = new List<IWorker>();

        [ObservableProperty]
        DateTime _BarStartTime;

        [ObservableProperty]
        DateTime _BarEndTime;

        [ObservableProperty]
        IEnumerable<FilteredWorkTimesModel>? _FilteredWorkTimesList;


        //public IReadOnlyCollection<IWorker> Workers => _workers;

        public EntireWorkTimeViewModel(ILogger<EntireWorkTimeViewModel> logger)
        {
            _logger = logger;

            WeakReferenceMessenger.Default.Register<WorkerListChangedMessage>(this, onWorkerListChanged);
            WeakReferenceMessenger.Default.Register<WorkerListLoadedMessage>(this, onWorkerListLoaded);
            WeakReferenceMessenger.Default.Register<WorkTimeFilterChangedMessage>(this, onWorkTimeFilterChanged);
        }
        
        public void Dispose()
        {
            WeakReferenceMessenger.Default.Unregister<WorkerListChangedMessage>(this);
            WeakReferenceMessenger.Default.Unregister<WorkerListLoadedMessage>(this);
            WeakReferenceMessenger.Default.Unregister<WorkTimeFilterChangedMessage>(this);

            _logger.LogInformation($"{nameof(EntireWorkTimeViewModel)} Disposed");
        }



        private void refreshWorkTimes()
        {
            if(!_workers.Any() || _currentFilter is null)
            {
                FilteredWorkTimesList = null;
                return;
            }

            FilteredWorkTimesList = _workers
                .Select(w => w.GetFilteredWorkTimes(_currentFilter.Year, _currentFilter.Month))
                .ToArray();
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

            refreshWorkTimes();
            //OnPropertyChanged(nameof(Workers));
        }
        private void onWorkerListLoaded(object sender, WorkerListLoadedMessage message)
        {
            _workers.Clear();

            WorkerListLoadedMessageArgs args = message.Value;
            foreach (var worker in args.Workers)
                _workers.Add(worker);

            refreshWorkTimes();
            //OnPropertyChanged(nameof(Workers));
        }
        private void onWorkTimeFilterChanged(object sender, WorkTimeFilterChangedMessage message)
        {
            _currentFilter = message.Value;
            
            BarStartTime = new DateTime(_currentFilter.Year, _currentFilter.Month, 1);
            BarEndTime = BarStartTime.AddMonths(1).AddDays(-1);

            refreshWorkTimes();
        }
    }
}
