using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
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
using WorkTimeTable.Infrastructure;
using WorkTimeTable.ViewModels;
using WorkTimeTable.Views;

namespace WorkTimeTable.Services
{
    public class SosoMessageBoxResult<TResult>
    {
        public MessageBoxResult MessageResult { get; init; }
        public TResult? Result { get; init; }

        public SosoMessageBoxResult(MessageBoxResult messageResult, TResult? result)
        {
            MessageResult = messageResult;
            Result = result;
        }
    }

    public interface ISosoMessageBoxService
    {
        SosoMessageBoxResult<TResult> Show<TView, TResult>(Window owner)
            where TView : SosoMessageBoxViewBase
            where TResult : class;
    }

    internal class SosoMessageBoxService : SosoMessageBox, ISosoMessageBoxService
    {
        readonly ILogger _logger;
        readonly IServiceProvider _svcProv;

        public SosoMessageBoxService(ILogger<SosoMessageBoxService> logger, IServiceProvider svcProv)
        {
            _logger = logger;
            _svcProv = svcProv;
        }

        public SosoMessageBoxResult<TResult> Show<TView, TResult>(Window owner) where TView : SosoMessageBoxViewBase where TResult : class
        {
            if(owner is null)
                throw new ArgumentNullException(nameof(owner));

            var view = Ioc.Default.GetRequiredService<TView>();
            var viewTypeSvc = Ioc.Default.GetRequiredService<IViewTypeService>();
            
            Type? viewModelType = viewTypeSvc.GetViewModelType(typeof(TView).Name);
            SosoMessageBoxViewModelBase<TResult>? viewModel = null;

            if (viewModelType is not null)
            {
                viewModel = Ioc.Default.GetService(viewModelType) as SosoMessageBoxViewModelBase<TResult>;
                if (viewModel is not null)
                    view.DataContext = viewModel;
            }

            Window msgWindow = SosoMessageBox.CreateWindow(owner);
            msgWindow.Content = view;

            if(view.MessageWindowStyle is not null)
                msgWindow.Style = view.MessageWindowStyle;

            msgWindow.ShowDialog();

            if (viewModel is null)
                return new SosoMessageBoxResult<TResult>(MessageBoxResult.OK, default);

            return new SosoMessageBoxResult<TResult>(
                viewModel is not null ? viewModel.MessageResult : MessageBoxResult.OK, viewModel?.Result);
        }
    }
}
