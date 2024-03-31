using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.ViewModels
{
    public partial class WorkTimesViewModel : ObservableObject
    {
        [ObservableProperty]
        IWorker? _TargetWorker;

        CollectionViewSource _WorkTimeSource;
        public ICollectionView WorkTimes => _WorkTimeSource.View;

        public WorkTimesViewModel()
        {
            _WorkTimeSource = new CollectionViewSource();
        }

        [RelayCommand]
        private void WorkTimeAddRequest()
        {
            if (TargetWorker == null)
                return;

            TargetWorker.WorkTimes.Add(new WorkTimeModel());
            _WorkTimeSource.View.Refresh();
        }

        partial void OnTargetWorkerChanged(IWorker? value)
        {
            if(value != null)
            {
                _WorkTimeSource.Source = new ObservableCollection<WorkTimeModel>(value.WorkTimes);
                //_WorkTimeSource.SortDescriptions.Add(new System.ComponentModel.SortDescription(nameof(WorkTimeModel.StartWorkTime), System.ComponentModel.ListSortDirection.Ascending));
            }
            else
            {
                _WorkTimeSource.Source = null;
                _WorkTimeSource.SortDescriptions.Clear();
            }

            _WorkTimeSource.View.Refresh();
        }
    }
}
