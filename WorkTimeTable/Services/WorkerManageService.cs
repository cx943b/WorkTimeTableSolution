using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration.Internal;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Services
{
    public interface IWorkerManageService
    {
        Task<IEnumerable<IWorker>?> LoadWorkersAsync();
        Task<bool> SaveWorkersAsync();
    }
    internal class WorkerManageService : IWorkerManageService
    {
        const string WorkerListPathKey = "WorkerListPath";

        readonly IConfiguration _configuration;
        readonly ILogger _logger;

        IReadOnlyCollection<WorkerModel>? _lastLoadedWorkers = null;

        public WorkerManageService(ILogger<WorkerManageService> logger, IConfiguration config)
        {
            _configuration = config;
            _logger = logger;
        }

        public async Task<IEnumerable<IWorker>?> LoadWorkersAsync()
        {
            string? listPath = _configuration.GetValue<string>(WorkerListPathKey);
            if(String.IsNullOrEmpty(listPath))
            {
                _logger.LogWarning("NotExist: WorkerListPath in Config");
                return null;
            }

            listPath = Directory.GetCurrentDirectory() + listPath;

            if (!File.Exists(listPath))
            {
                _logger.LogWarning($"NotFound: {nameof(listPath)}");
                return null;
            }

            string jsonStr = await File.ReadAllTextAsync(listPath);
            if (String.IsNullOrEmpty(jsonStr))
            {
                _logger.LogWarning($"EmptyFile: {nameof(listPath)}");
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.ReadCommentHandling = JsonCommentHandling.Skip;
            options.Converters.Add(new SolidColorBrushJsonConverter());

            try
            {
                _lastLoadedWorkers = JsonSerializer.Deserialize<WorkerModel[]>(jsonStr, options)?.AsReadOnly();
                return _lastLoadedWorkers;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Deserialize Error");
                return null;
            }
        }
        public async Task<bool> SaveWorkersAsync()
        {
            if(_lastLoadedWorkers is null || !_lastLoadedWorkers.Any())
            {
                _logger.LogWarning("Empty workers save passed");
                return true;
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new SolidColorBrushJsonConverter());
            options.WriteIndented = true;

            try
            {
                string jsonWorkersStr = JsonSerializer.Serialize<IEnumerable<WorkerModel>>(_lastLoadedWorkers, options);
                string filePath = Directory.GetCurrentDirectory() + _configuration.GetValue<string>(WorkerListPathKey);

                await File.WriteAllTextAsync(filePath, jsonWorkersStr);

                _logger.LogInformation($"WorkerList saved");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Save Error");
                return false;
            }
        }
    }
}
