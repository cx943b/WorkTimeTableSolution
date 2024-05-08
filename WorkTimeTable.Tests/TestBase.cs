using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable.Tests
{
    public class MvvmTestBase
    {
        readonly ILoggerFactory _logFac;
        readonly IConfiguration _config;

        public MvvmTestBase()
        {
            _logFac = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
            });

            _config = (new ConfigurationBuilder())
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IConfiguration GetConfig() => _config;
        public ILogger<T> CreateLogger<T>() => _logFac.CreateLogger<T>();
    }
}
