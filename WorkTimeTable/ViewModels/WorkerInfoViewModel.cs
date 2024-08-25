using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using SosoThemeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Messages;

namespace WorkTimeTable.ViewModels
{
    public partial class WorkerInfoViewModel : ObservableObject, IDisposable
    {
        IWorker? _TargetWorker;


        [ObservableProperty]
        string _Name = "Kill-Dong Hong";
        [ObservableProperty]
        string _BirthDate = "000000";
        [ObservableProperty]
        string _ColorName = $"{nameof(Colors.CornflowerBlue)}";


        public WorkerInfoViewModel()
        {
            WeakReferenceMessenger.Default.Register<TargetWorkerChangedMessage>(this, onTargetWorkerChanged);
        }

        public void Dispose()
        {
            WeakReferenceMessenger.Default.Unregister<TargetWorkerChangedMessage>(this);
        }

        partial void OnNameChanged(string value)
        {
            if (_TargetWorker != null)
            {
                _TargetWorker.Name = value;
            }
        }
        partial void OnBirthDateChanged(string value)
        {
            if (_TargetWorker != null)
            {
                _TargetWorker.BirthDate = value;
            }
        }
        partial void OnColorNameChanged(string value)
        {
            if (_TargetWorker != null)
            {
                _TargetWorker.ColorName = value;

                // Surpport only Color at this time(240617)
                WeakReferenceMessenger.Default.Send(new TargetWorkerInfoChangedMessage(_TargetWorker));
            }
        }

        private void onTargetWorkerChanged(object sender ,TargetWorkerChangedMessage msg)
        {
            _TargetWorker = msg.Value;

            if(_TargetWorker == null)
            {
                Name = "Kill-Dong Hong";
                BirthDate = "000000";
                ColorName = $"{nameof(Colors.CornflowerBlue)}";
            }
            else
            {
                Name = _TargetWorker.Name;
                BirthDate = _TargetWorker.BirthDate;
                ColorName = _TargetWorker.ColorName;
            }
        }
    }
}
