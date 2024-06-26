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

namespace WorkTimeTable.ViewModels
{
    public partial class SosoMessageBoxViewModelBase<TResult> : ObservableValidator, ISosoMessageBoxViewModel where TResult : class
    {
        [ObservableProperty]
        string _Title = "";

        [ObservableProperty]
        MessageBoxResult _MessageResult = MessageBoxResult.Cancel;

        public TResult? Result { get; protected set; } = null;

        [RelayCommand]
        protected virtual void OnInitialized(EventArgs e) { }


        [RelayCommand]
        protected virtual void OnClosing(SosoMessageCloseEventArgs e) { }
    }
}
