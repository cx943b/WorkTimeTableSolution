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
using SosoThemeLibrary.Controls;
using WorkTimeTable.Views;

namespace WorkTimeTable
{
    public partial class App : Application
    {
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
                builder.ClearProviders();
                builder.AddConsole();
            });
            svcProv.AddSingleton<IConfiguration>(config);
            svcProv.AddSingleton<IWorkerManageService, WorkerManageService>();

            svcProv.AddSingleton<ViewModels.EntireWorkerTimeViewModel>();
            svcProv.AddSingleton<ViewModels.MainViewModel>();

            svcProv.AddScoped<Window>(prov => createWindow());

            return svcProv.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _svcProv.GetRequiredService<Window>();
            mainWindow.Content = new MainView();

            MainWindow = mainWindow;
            mainWindow.Show();
        }

        private Window createWindow()
        {
            var w = new SosoWindow();
            w.Style = (Style)Resources["MainWindowStykeKey"];

            return w;
        }
    }
}
