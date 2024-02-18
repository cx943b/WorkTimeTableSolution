using CommunityToolkit.Mvvm.DependencyInjection;
using SosoThemeLibrary;
using SosoThemeLibrary.Controls;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkTimeTable.ViewModels;
using WorkTimeTable.Views;

namespace WorkTimeTable.Services
{
    internal class SosoMessageBoxService : SosoMessageBox
    {
        public static MessageBoxResult Show<TView, TViewModel>(Window owner)
            where TView : SosoMessageBoxViewBase
            where TViewModel : SosoMessageBoxViewModelBase
        {
            if(owner is null)
                throw new ArgumentNullException(nameof(owner));

            var view = Ioc.Default.GetRequiredService<TView>();
            if(view is null)
                throw new TypeLoadException($"NotFound: {typeof(TView)}");

            var viewModel = Ioc.Default.GetRequiredService<TViewModel>();
            if (viewModel is null)
                throw new TypeLoadException($"NotFound: {typeof(TViewModel)}");

            view.DataContext = viewModel;

            Window msgWindow = SosoMessageBox.CreateWindow(owner);
            msgWindow.Content = view;

            if(view.MessageWindowStyle is not null)
                msgWindow.Style = view.MessageWindowStyle;

            msgWindow.ShowDialog();

            return viewModel.MessageResult;
        }
    }
}
