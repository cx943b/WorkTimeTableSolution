﻿using CommunityToolkit.Mvvm.ComponentModel;
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
        readonly ObservableCollection<IWorkTime> _workTimeColl = new ObservableCollection<IWorkTime>();

        WorkTimeFilter _currentWorkTimeFilter = new WorkTimeFilter(DateTime.Now.Year, DateTime.Now.Month);

        [ObservableProperty]
        IEnumerable<int> _DaysInMonth;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(WorkTimeAddRequestCommand))]
        [NotifyPropertyChangedFor(nameof(TargetWorkTimes))]
        [property:ReadOnly(true)]
        IWorker? _TargetWorker;


        readonly CollectionViewSource _cvsWorkTimeCollSorter;
        public ICollectionView TargetWorkTimes => _cvsWorkTimeCollSorter.View;

        public WorkTimesViewModel()
        {
            _cvsWorkTimeCollSorter = new CollectionViewSource();
            _cvsWorkTimeCollSorter.Source = _workTimeColl;
            _cvsWorkTimeCollSorter.SortDescriptions.Add(new SortDescription("Day", ListSortDirection.Ascending));

            _DaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(_currentWorkTimeFilter.Year, _currentWorkTimeFilter.Month));

            WeakReferenceMessenger.Default.Register<WorkTimeFilterChangedMessage>(this, onWorkTimeFilterChanged);
            WeakReferenceMessenger.Default.Register<TargetWorkerChangedMessage>(this, onTargetWorkerChangedByService);
        }

        public void Dispose()
        {
            WeakReferenceMessenger.Default.Unregister<WorkTimeFilterChangedMessage>(this);
            WeakReferenceMessenger.Default.Unregister<TargetWorkerChangedMessage>(this);
        }

        private void reLoadWorkTimes()
        {
            _workTimeColl.Clear();

            if (TargetWorker != null)
            {
                var filteredWorkTimes = TargetWorker.GetFilteredWorkTimes(_currentWorkTimeFilter.Year, _currentWorkTimeFilter.Month);
                foreach (var workTime in filteredWorkTimes.WorkTimes)
                {
                    _workTimeColl.Add(workTime);
                }
            }
            
            TargetWorkTimes.Refresh();
        }

        private bool CanWorkTimeAddRequest() => TargetWorker != null;

        [RelayCommand(CanExecute = nameof(CanWorkTimeAddRequest))]
        private void WorkTimeAddRequest()
        {
            if (TargetWorker == null)
                throw new ArgumentNullException(nameof(TargetWorker));

            int lastDay = _workTimeColl.Max(time => time.StartWorkTime.Day) + 1;
            lastDay = Math.Min(lastDay, DateTime.DaysInMonth(_currentWorkTimeFilter.Year, _currentWorkTimeFilter.Month));

            var newWorkTime = new WorkTimeModel() { Year = _currentWorkTimeFilter.Year, Month = _currentWorkTimeFilter.Month, Day = lastDay };

            TargetWorker.AddWorkTime(newWorkTime);
            _workTimeColl.Add(newWorkTime);

            //TargetWorkTimes.Refresh();
        }

        [RelayCommand]
        private void WorkTimeRemoveRequest(IWorkTime targetWorkTime)
        {
            if (TargetWorker == null)
                throw new ArgumentNullException(nameof(TargetWorker));

            TargetWorker.RemoveWorkTime((WorkTimeModel)targetWorkTime);
            _workTimeColl.Remove(targetWorkTime);

            TargetWorkTimes.Refresh();
        }

        [RelayCommand]
        private void ScrollChanged(ScrollChangedEventArgs e)
        {
            Debug.WriteLine("ddd");
        }

        private void onWorkTimeFilterChanged(object sender, WorkTimeFilterChangedMessage message)
        {
            _currentWorkTimeFilter = message.Value;
            DaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(_currentWorkTimeFilter.Year, _currentWorkTimeFilter.Month));

            reLoadWorkTimes();
        }

        private void onTargetWorkerChangedByService(object sender, TargetWorkerChangedMessage message)
        {
            TargetWorker = message.Value;
            reLoadWorkTimes();
        }

        partial void OnTargetWorkerChanged(IWorker? value)
        {
            reLoadWorkTimes();
        }


    }
}
