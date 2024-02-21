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

namespace WorkTimeTable
{
    public partial class App : Application
    {
        const string LogTemplate = "{Timestamp:HH:mm:ss} [{Level:u4}] {Message:l}{NewLine}{Exception}";
        readonly IServiceProvider _svcProv;

        public App()
        {
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
            svcProv.AddSingleton<IWorkerManageService, WorkerManageService>();

            svcProv.AddSingleton<Views.AddWorkerView>();

            svcProv.AddSingleton<ViewModels.EntireWorkerTimeViewModel>();
            svcProv.AddSingleton<ViewModels.LoadWorkerListViewModel>();
            svcProv.AddSingleton<ViewModels.AddWorkerViewModel>();
            svcProv.AddSingleton<ViewModels.MainViewModel>();

            svcProv.AddTransient<Window>(prov => createWindow());

            return svcProv.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _svcProv.GetRequiredService<Window>();
            mainWindow.Content = new MainView();
            mainWindow.Width = 800;
            mainWindow.Height = 600;
            mainWindow.Loaded += async (s, e) =>
            {
                var workerMgrSvc = _svcProv.GetRequiredService<IWorkerManageService>();
                await workerMgrSvc.LoadWorkersAsync();
            };
            mainWindow.Closed += async (s, e) =>
            {
                var workerMgrSvc = _svcProv.GetRequiredService<IWorkerManageService>();
                await workerMgrSvc.SaveWorkersAsync();
            };

            MainWindow = mainWindow;
            mainWindow.Show();

            

            //var addWindow = _svcProv.GetRequiredService<Window>();
            //addWindow.Content = new AddWorkerView();
            //addWindow.Width = 352;
            //addWindow.Height = 176;
            //addWindow.Title = "New Worker Info";
            //addWindow.Show();
        }

        private Window createWindow()
        {
            var w = new SosoWindow();
            w.Style = (Style)Resources["MainWindowStyleKey"];

            return w;
        }
    }
}
