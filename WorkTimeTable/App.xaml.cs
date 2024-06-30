using System.Configuration;
using System.Data;
using System.Windows;

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkTimeTable.Services;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using System.IO;

using Serilog.Configuration;
using Serilog.Extensions.Logging;
using Serilog.Sinks.File;
using Serilog;

using SosoThemeLibrary.Controls;
using WorkTimeTable.Views;
using CommunityToolkit.Mvvm.Messaging;
using Serilog.Core;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Messages;
using System.Resources;
using System.Globalization;
using WorkTimeTable.ViewModels;
using WorkTimeTable.Infrastructure;

namespace WorkTimeTable
{
    public partial class App : Application
    {
        const string LogTemplate = "{Timestamp:HH:mm:ss} [{Level:u4}] {Message:l}{NewLine}{Exception}";
        readonly IServiceProvider _svcProv;

        public App()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            _svcProv = CreateServiceProvider();
            Ioc.Default.ConfigureServices(_svcProv);
        }

        private IServiceProvider CreateServiceProvider()
        {
            var config =  (new ConfigurationBuilder())
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var svcProv = new ServiceCollection();
            svcProv.AddLogging(builder =>
            {
                var logConfig = new LoggerConfiguration()
                    .WriteTo.File($"logs/log-{DateTime.Now:yyyy-MM-dd}.log",
                              outputTemplate: LogTemplate,
                              fileSizeLimitBytes: 100_000_000,
                              rollOnFileSizeLimit: true)
                    .WriteTo.Console();

                builder.ClearProviders();
                builder.AddSerilog(logConfig.CreateLogger());
            });
            svcProv.AddSingleton<IConfiguration>(config);
            svcProv.AddSingleton<IViewTypeService, TypeService>();
            svcProv.AddSingleton<ITypeService, TypeService>();
            svcProv.AddSingleton<ISosoMessageBoxService, SosoMessageBoxService>();
            svcProv.AddSingleton<IWorkerManageService, WorkerManageService>();

            // Views
            svcProv.AddSingleton<LoadWorkerListView>();
            svcProv.AddSingleton<EntireWorkTimeView>();
            svcProv.AddSingleton<WorkTimesView>();
            svcProv.AddSingleton<WorkTimeFilterView>();
            svcProv.AddSingleton<AddWorkerView>();
            svcProv.AddSingleton<WorkerInfoView>();
            svcProv.AddSingleton<WorkerListView>();
            svcProv.AddSingleton<MainView>();

            // ViewModels
            svcProv.AddSingleton<WorkTimesViewModel>();
            svcProv.AddSingleton<EntireWorkTimeViewModel>();
            svcProv.AddSingleton<LoadWorkerListViewModel>();
            svcProv.AddSingleton<AddWorkerViewModel>();
            svcProv.AddSingleton<WorkTimeFilterViewModel>();
            svcProv.AddSingleton<MainViewModel>();
            svcProv.AddSingleton<WorkerInfoViewModel>();
            svcProv.AddSingleton<WorkerListViewModel>();

            // MessageBox
            svcProv.AddTransient<AddWorkerMessageBoxView>();
            svcProv.AddTransient<AddWorkerMessageBoxViewModel>();

            svcProv.AddTransient(prov => createWindow());
            return svcProv.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _svcProv.GetRequiredService<Window>();
            mainWindow.Content = _svcProv.GetRequiredService<MainView>();
            mainWindow.Width = 900;
            mainWindow.Height = 600;
            mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mainWindow.Loaded += async (s, e) =>
            {
                var workerMgrSvc = _svcProv.GetRequiredService<IWorkerManageService>();
                await workerMgrSvc.LoadWorkersAsync();

                workerMgrSvc.InitializeFilter(2024, 1);

                if(workerMgrSvc.LastLoadedWorkers is not null)
                {
                    workerMgrSvc.TargetWorker = workerMgrSvc.LastLoadedWorkers.FirstOrDefault();
                }
            };
            mainWindow.Closed += async (s, e) =>
            {
                var workerMgrSvc = _svcProv.GetRequiredService<IWorkerManageService>();
                await workerMgrSvc.SaveWorkersAsync();
            };

            MainWindow = mainWindow;
            mainWindow.Show();
        }

        private Window createWindow()
        {
            var w = new SosoWindow();
            w.Style = (Style)Resources["MainWindowStyleKey"];

            return w;
        }
    }
}
