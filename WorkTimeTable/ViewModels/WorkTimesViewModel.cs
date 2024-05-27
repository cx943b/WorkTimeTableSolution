using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using WorkTimeTable.Controls;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Messages;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.ViewModels
{
    public partial class WorkTimesViewModel : ObservableObject, IDisposable
    {
        static readonly IEnumerable<int> _TargetMonths = Enumerable.Range(1, 12).ToArray();
        readonly ObservableCollection<IWorkTime> _workTimeColl = new ObservableCollection<IWorkTime>();

        WorkTimeFilter _currentWorkTimeFilter = new WorkTimeFilter(DateTime.Now.Year, DateTime.Now.Month);


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(WorkTimeAddRequestCommand))]
        [NotifyPropertyChangedFor(nameof(WorkTimes))]
        [property:ReadOnly(true)]
        IWorker? _TargetWorker;                

        public IEnumerable<int> TargetMonths => _TargetMonths;

        readonly CollectionViewSource _WorkTimeSource;
        public ICollectionView WorkTimes => _WorkTimeSource.View;

        public WorkTimesViewModel()
        {
            _WorkTimeSource = new CollectionViewSource();
            _WorkTimeSource.Source = _workTimeColl;
            _WorkTimeSource.SortDescriptions.Add(new SortDescription("Day", ListSortDirection.Ascending));

            WeakReferenceMessenger.Default.Register<WorkTimeFilterChangedMessage>(this, onWorkTimeFilterChanged);
            WeakReferenceMessenger.Default.Register<TargetWorkerChangedMessage>(this, onTargetWorkerChangedByService);
        }

        public void Dispose()
        {
            WeakReferenceMessenger.Default.Unregister<WorkTimeFilterChangedMessage>(this);
            WeakReferenceMessenger.Default.Unregister<TargetWorkerChangedMessage>(this);
        }

        private void refreshWorkTimes()
        {
            _workTimeColl.Clear();

            if (TargetWorker != null)
            {
                var filteredWorkTimes = TargetWorker.GetFilteredWorkTimes(_currentWorkTimeFilter.Year, _currentWorkTimeFilter.Month);
                if(filteredWorkTimes.WorkTimes.Any())
                {
                    foreach (var workTime in filteredWorkTimes.WorkTimes)
                    {
                        _workTimeColl.Add(workTime);
                    }
                }
            }

            WorkTimes.Refresh();
        }

        private bool CanWorkTimeAddRequest() => TargetWorker != null;

        [RelayCommand(CanExecute = nameof(CanWorkTimeAddRequest))]
        private void WorkTimeAddRequest()
        {
            if (TargetWorker == null)
                throw new ArgumentNullException(nameof(TargetWorker));

            var newWorkTime = new WorkTimeModel() { Year = _currentWorkTimeFilter.Year, Month = _currentWorkTimeFilter.Month, Day = 1 };

            TargetWorker.AddWorkTime(newWorkTime);
            _workTimeColl.Add(newWorkTime);

            WorkTimes.Refresh();
        }

        [RelayCommand]
        private void WorkTimeRemoveRequest(IWorkTime targetWorkTime)
        {
            if (TargetWorker == null)
                throw new ArgumentNullException(nameof(TargetWorker));

            TargetWorker.RemoveWorkTime((WorkTimeModel)targetWorkTime);
            _workTimeColl.Remove(targetWorkTime);

            WorkTimes.Refresh();
        }

        [RelayCommand]
        private void ScrollChanged(ScrollChangedEventArgs e)
        {
            Debug.WriteLine("ddd");
        }

        private void onWorkTimeFilterChanged(object sender, WorkTimeFilterChangedMessage message)
        {
            _currentWorkTimeFilter = message.Value;
            refreshWorkTimes();
        }

        private void onTargetWorkerChangedByService(object sender, TargetWorkerChangedMessage message)
        {
            TargetWorker = message.Value;
        }

        partial void OnTargetWorkerChanged(IWorker? value)
        {
            refreshWorkTimes();
        }


    }
}
