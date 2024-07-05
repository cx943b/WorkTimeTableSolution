using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SosoThemeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Services;
using WorkTimeTable.Views;

namespace WorkTimeTable.ViewModels
{
    partial class RemoveWorkerViewModel : ObservableObject
    {
        [RelayCommand]
        private void OnRemoveButtonClicked()
        {
            var msgResult = SosoMessageBox.Show(App.Current.MainWindow, "Remove?", "Remove selected worker", MessageBoxImage.Question, MessageBoxButton.YesNo);

            if (msgResult == MessageBoxResult.OK)
            {
            }
        }
    }
}
