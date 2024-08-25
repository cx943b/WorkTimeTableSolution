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
using System.Windows.Data;
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
        readonly IWorkerManageService _workerMgrSvc;

        [ObservableProperty]
        DateTime _BarStartTime;

        [ObservableProperty]
        DateTime _BarEndTime;

        readonly ObservableCollection<FilteredWorkTimesModel> _lstFilteredWorkTimes = new ObservableCollection<FilteredWorkTimesModel>();
        readonly CollectionViewSource _cvsFilteredWorkTimes = new CollectionViewSource();
        public ICollectionView FilteredWorkTimesList => _cvsFilteredWorkTimes.View;

        //public IReadOnlyCollection<IWorker> Workers => _workers;

        public EntireWorkTimeViewModel(ILogger<EntireWorkTimeViewModel> logger, IWorkerManageService workerMgrSvc)
        {
            _logger = logger;
            _workerMgrSvc = workerMgrSvc;

            _cvsFilteredWorkTimes.Source = _lstFilteredWorkTimes;

            WeakReferenceMessenger.Default.Register<WorkerListChangedMessage>(this, onWorkerListChanged);
            WeakReferenceMessenger.Default.Register<WorkerListLoadedMessage>(this, onWorkerListLoaded);
            WeakReferenceMessenger.Default.Register<WorkTimeFilterChangedMessage>(this, onWorkTimeFilterChanged);
            WeakReferenceMessenger.Default.Register<TargetWorkerInfoChangedMessage>(this, onTargetWorkerInfoChanged);
        }
        
        public void Dispose()
        {
            WeakReferenceMessenger.Default.Unregister<WorkerListChangedMessage>(this);
            WeakReferenceMessenger.Default.Unregister<WorkerListLoadedMessage>(this);
            WeakReferenceMessenger.Default.Unregister<WorkTimeFilterChangedMessage>(this);
            WeakReferenceMessenger.Default.Unregister<TargetWorkerInfoChangedMessage>(this);

            _logger.LogInformation($"{nameof(EntireWorkTimeViewModel)} Disposed");
        }



        private void refreshWorkTimes()
        {
            _lstFilteredWorkTimes.Clear();

            if (_workerMgrSvc.LastLoadedWorkers == null || _currentFilter is null)
                return;

            var filteredWorkTimes = _workerMgrSvc.LastLoadedWorkers
                .Select(w => w.GetFilteredWorkTimes(_currentFilter.Year, _currentFilter.Month));

            foreach (var worker in filteredWorkTimes)
            {
                _lstFilteredWorkTimes.Add(worker);
            }
        }

        private void onTargetWorkerInfoChanged(object sender, TargetWorkerInfoChangedMessage message)
        {
            var worker = message.Value;
            var filteredWorkTimes = _lstFilteredWorkTimes.FirstOrDefault(workTime => workTime.WorkerId == worker.Id);

            if (filteredWorkTimes != null)
            {
                filteredWorkTimes.ColorName = worker.ColorName;
            }
        }
        private void onWorkerListChanged(object sender, WorkerListChangedMessage message)
        {
            WorkerListChangedMessageArgs args = message.Value;

            if (args.Status == WorkerListChangedStatus.Added)
            {
                var filteredWorkTimes = args.Workers
                    .Select(w => w.GetFilteredWorkTimes(_currentFilter.Year, _currentFilter.Month))
                    .ToArray();

                Array.ForEach(filteredWorkTimes, _lstFilteredWorkTimes.Add);
            }
            else if (args.Status == WorkerListChangedStatus.Removed)
            {
                foreach(var worker in args.Workers)
                {
                    var filteredWorkTimes = _lstFilteredWorkTimes.FirstOrDefault(workTime => workTime.WorkerId == worker.Id);
                    if (filteredWorkTimes != null)
                    {
                        _lstFilteredWorkTimes.Remove(filteredWorkTimes);
                    }   
                }
            }

            //refreshWorkTimes();
        }
        private void onWorkerListLoaded(object sender, WorkerListLoadedMessage message)
        {
            refreshWorkTimes();
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
