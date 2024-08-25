using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SosoThemeLibrary;
using SosoThemeLibrary.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;
using WorkTimeTable.Views;

namespace WorkTimeTable.ViewModels
{
    internal partial class AddWorkerViewModel : ObservableObject
    {
        [RelayCommand]
        private void OnAddButtonClicked()
        {
            ISosoMessageBoxService msgSvc = Ioc.Default.GetRequiredService<ISosoMessageBoxService>();
            var msgResult = msgSvc.Show<AddWorkerMessageBoxView, IWorker>(App.Current.MainWindow);

            if (msgResult.MessageResult == System.Windows.MessageBoxResult.OK)
            {
                if (msgResult.Result is IWorker worker)
                {
                    var workerMgrSvc = Ioc.Default.GetRequiredService<IWorkerManageService>();
                    workerMgrSvc.TryAddWorker(worker);
                }
            }
        }
    }
}
