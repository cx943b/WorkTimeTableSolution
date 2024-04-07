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
        [NotifyCanExecuteChangedFor(nameof(WorkTimeAddRequestCommand))]
        [NotifyPropertyChangedFor(nameof(WorkTimes))]
        IWorker? _TargetWorker;

        [ObservableProperty]
        int _TargetYear = DateTime.Now.Year;

        [ObservableProperty]
        int _TargetMonth = 1; // DateTime.Now.Month;

        readonly CollectionViewSource _WorkTimeSource;
        public ICollectionView? WorkTimes { get; private set; }

        public WorkTimesViewModel()
        {
            _WorkTimeSource = new CollectionViewSource();
            _WorkTimeSource.Filter += new FilterEventHandler((s, e) =>
            {
                if (e.Item is WorkTimeModel item)
                {
                    e.Accepted = item.Year == TargetYear && item.Month == TargetMonth;
                }
            });
        }

        private bool CanWorkTimeAddRequest() => TargetWorker != null;

        [RelayCommand(CanExecute = nameof(CanWorkTimeAddRequest))]
        private void WorkTimeAddRequest()
        {
            if (TargetWorker == null)
                return;

            TargetWorker.WorkTimes.Add(new WorkTimeModel() { Year = 2024, Month = 1, Day = 1 });
            WorkTimes.Refresh();
        }

        [RelayCommand]
        private void WorkTimeRemoveRequest(IWorkTime targetWorkTime)
        {
            if (TargetWorker == null)
                throw new ArgumentNullException(nameof(TargetWorker));

            TargetWorker.WorkTimes.Remove((WorkTimeModel)targetWorkTime);
            WorkTimes.Refresh();
        }

        partial void OnTargetWorkerChanged(IWorker? value)
        {
            if(value != null)
            {
                _WorkTimeSource.Source = value.WorkTimes;
                WorkTimes = _WorkTimeSource.View;
                //WorkTimes.Refresh();

            }
            else
            {
                _WorkTimeSource.Source = null;
                WorkTimes = null;
            }
        }
    }
}
