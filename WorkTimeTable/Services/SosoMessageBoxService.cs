﻿using CommunityToolkit.Mvvm.DependencyInjection;
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
using WorkTimeTable.ViewModels;
using WorkTimeTable.Views;

namespace WorkTimeTable.Services
{
    public interface ISosoMessageBoxService
    {
        MessageBoxResult Show<TView, TViewModel>(Window owner)
            where TView : SosoMessageBoxViewBase
            where TViewModel : SosoMessageBoxViewModelBase;
        MessageBoxResult ShowAddWorkTimeView(Window owner, int workerId);
    }

    internal class SosoMessageBoxService : SosoMessageBox, ISosoMessageBoxService
    {
        readonly ILogger _logger;

        public SosoMessageBoxService(ILogger<SosoMessageBoxService> logger)
        {
            _logger = logger;
        }

        public MessageBoxResult Show<TView, TViewModel>(Window owner)
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

        public MessageBoxResult ShowAddWorkTimeView(Window owner, int workerId)
        {
            if (owner is null)
                throw new ArgumentNullException(nameof(owner));

            var viewModel = Ioc.Default.GetRequiredService<AddWorkTimeViewModel>();
            if (viewModel is null)
                throw new TypeLoadException($"NotFound: {typeof(AddWorkTimeViewModel)}");
            
            bool isWorkerReady = viewModel.SetWorkerId(workerId);
            if(!isWorkerReady)
            {
                _logger.LogWarning($"InvalidWorkerId: {workerId}");
                return MessageBoxResult.Cancel;
            }

            var view = Ioc.Default.GetRequiredService<AddWorkTimeView>();
            if (view is null)
                throw new TypeLoadException($"NotFound: {typeof(AddWorkTimeView)}");

            view.DataContext = viewModel;

            Window msgWindow = SosoMessageBox.CreateWindow(owner);
            msgWindow.Content = view;

            if (view.MessageWindowStyle is not null)
                msgWindow.Style = view.MessageWindowStyle;

            msgWindow.ShowDialog();

            return viewModel.MessageResult;
        }
    }
}
