using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Controls;
using WorkTimeTable.Infrastructure;

namespace WorkTimeTable.ViewModels
{
    public partial class WorkTimeFilterViewModel : ObservableObject, IDisposable
    {
        bool _isChangedByService = false;

        [ObservableProperty]
        int _TargetYear = DateTime.Now.Year;

        [ObservableProperty]
        int _TargetMonth = DateTime.Now.Month;

        [ObservableProperty]
        IEnumerable<int> _TargetMonths = Enumerable.Range(1, 12);

        public WorkTimeFilterViewModel()
        {
            WeakReferenceMessenger.Default.Register<WorkTimeFilterChangedMessage>(this, onFilterChangedByService);
        }

        public void Dispose()
        {
            WeakReferenceMessenger.Default.Unregister<WorkTimeFilterChangedMessage>(this);
        }

        partial void OnTargetMonthChanged(int value) => onFilterChanged();

        partial void OnTargetYearChanged(int value) => onFilterChanged();

        private void onFilterChanged()
        {
            if(_isChangedByService)
            {
                _isChangedByService = false;
                return;
            }

            WorkTimeFilterChangedMessage msg = new WorkTimeFilterChangedMessage(new WorkTimeFilter(TargetYear, TargetMonth));
            WeakReferenceMessenger.Default.Send<WorkTimeFilterChangedMessage>(msg);
        }
        private void onFilterChangedByService(object sender, WorkTimeFilterChangedMessage message)
        {
            _isChangedByService = true;
        }
    }
}
