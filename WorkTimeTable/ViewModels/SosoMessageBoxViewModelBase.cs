using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SosoThemeLibrary;
using SosoThemeLibrary.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.ViewModels
{
    public partial class SosoMessageBoxViewModelBase : ObservableValidator, ISosoMessageBoxViewModel
    {
        [ObservableProperty]
        string _Title = "";

        [ObservableProperty]
        MessageBoxResult _MessageResult = MessageBoxResult.Cancel;

        [RelayCommand]
        protected virtual void OnInitialized(EventArgs e) { }


        [RelayCommand]
        protected virtual void OnClosing(SosoMessageCloseEventArgs e) { }
    }

    public class AddWorkerMessageBoxViewModel : SosoMessageBoxViewModelBase
    {
        public WorkerModel? NewWorker { get; set; }
    }
}
