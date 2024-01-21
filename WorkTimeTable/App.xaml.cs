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

namespace WorkTimeTable
{
    public partial class App : Application
    {
        public App()
        {
            IServiceProvider svcProv = CreateServiceProvider();
            Ioc.Default.ConfigureServices(svcProv);
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
            


            return svcProv.BuildServiceProvider();
        }
    }
}
