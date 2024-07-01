using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Messages;

namespace WorkTimeTable.ViewModels
{
    internal partial class WorkerListViewModel : ObservableObject
    {
        readonly ObservableCollection<IWorker> _workerColl = new ObservableCollection<IWorker>();
        readonly CollectionViewSource _cvsWorkerColl = new CollectionViewSource();

        public ICollectionView Workers => _cvsWorkerColl.View;


        public WorkerListViewModel()
        {
            _cvsWorkerColl.Source = _workerColl;
            _cvsWorkerColl.SortDescriptions.Add(new SortDescription(nameof(IWorker.Id), ListSortDirection.Ascending));

            WeakReferenceMessenger.Default.Register<WorkerListLoadedMessage>(this, onWorkerListLoaded);
            WeakReferenceMessenger.Default.Register<WorkerListChangedMessage>(this, onWorkerListChanged);
        }

        [RelayCommand]
        private void onWorkerSelectionChanged(IWorker selectedWorker)
        {
            WeakReferenceMessenger.Default.Send(new TargetWorkerChangedMessage(selectedWorker));
        }

        private void onWorkerListLoaded(object sender, WorkerListLoadedMessage message)
        {
            _workerColl.Clear();
            foreach (var worker in message.Value.Workers)
            {
                _workerColl.Add(worker);
            }
        }

        private void onWorkerListChanged(object sender, WorkerListChangedMessage message)
        {
            var args = message.Value;

            if(args.Status == WorkerListChangedStatus.Added)
            {
                foreach (var worker in args.Workers)
                {
                    _workerColl.Add(worker);
                }
            }
            else
            {
                foreach (var worker in args.Workers)
                {
                    _workerColl.Remove(worker);
                }
            }

            _cvsWorkerColl.View.Refresh();
        }
    }
}
