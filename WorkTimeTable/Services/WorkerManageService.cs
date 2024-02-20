using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
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
using System.Windows;
using System.Windows.Media;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Messages;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Services
{
    public interface IWorkerManageService
    {
        bool IsExistWorker(string name, string birthDate);
        Task<IEnumerable<IWorker>?> LoadWorkersAsync();
        Task<bool> SaveWorkersAsync();
        bool TryAddWorker(string name, string birthDate, SolidColorBrush brush, DayOfWeekFlag fixedWorkWeeks, out WorkerModel? newWorker);
    }

    internal class WorkerManageService : IWorkerManageService
    {
        const string WorkerListPathKey = "WorkerListPath";

        readonly IConfiguration _configuration;
        readonly ILogger _logger;

        IList<WorkerModel>? _lastLoadedWorkers = null;

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
                _lastLoadedWorkers = JsonSerializer.Deserialize<WorkerModel[]>(jsonStr, options)?.ToList();
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
            if (_lastLoadedWorkers is null)
                throw new NullReferenceException(nameof(_lastLoadedWorkers));

            if (!_lastLoadedWorkers.Any())
            {
                _logger.LogWarning("Empty workers, save passed");
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
        public bool IsExistWorker(string name, string birthDate)
        {
            if (_lastLoadedWorkers is null)
                throw new NullReferenceException(nameof(_lastLoadedWorkers));
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if(String.IsNullOrEmpty(birthDate))
                throw new ArgumentNullException(nameof(birthDate));

            return _lastLoadedWorkers.Any(m => String.Compare(m.Name, name) == 0 && String.Compare(m.BirthDate, birthDate) == 0);
        }
        public bool TryAddWorker(string name, string birthDate, SolidColorBrush brush, DayOfWeekFlag fixedWorkWeeks, out WorkerModel? newWorker)
        {
            if (_lastLoadedWorkers is null)
                throw new NullReferenceException(nameof(_lastLoadedWorkers));
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (String.IsNullOrEmpty(birthDate))
                throw new ArgumentNullException(nameof(birthDate));
            if(brush is null)
                throw new ArgumentNullException(nameof(brush));

            newWorker = null;

            int newId = nextNewId();
            if(newId < 0)
            {
                _logger.LogError($"InvalidId: {nameof(newId)}");
                return false;
            }

            if(!DateTime.TryParseExact(birthDate, "yyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime birthDateValue))
            {
                _logger.LogError($"InvalidBirthDate: {birthDate}");
                return false;
            }

            var isExist = IsExistWorker(name, birthDate);
            if (isExist)
            {
                _logger.LogWarning($"AlreadyExist: {name}, {birthDate}");
                return false;
            }

            newWorker = new WorkerModel(newId, name, brush, fixedWorkWeeks);
            _lastLoadedWorkers.Add(newWorker);

            _logger.LogInformation($"Added new worker: {newWorker}");
            WeakReferenceMessenger.Default.Send(new WorkerListChangedMessageArgs(WorkerListChangedStatus.Added, new WorkerModel[] { newWorker }));

            return true;
        }

        private int nextNewId(bool isFillBlank = true)
        {
            if (_lastLoadedWorkers is null)
                throw new NullReferenceException(nameof(_lastLoadedWorkers));

            if (isFillBlank)
            {
                var ids = Enumerable.Range(1, _lastLoadedWorkers.Count);
                foreach((IWorker model, int id) in _lastLoadedWorkers.OrderBy(m => m.Id).Zip(ids))
                {
                    if (model.Id != id)
                        return id;
                }
            }

            return _lastLoadedWorkers.Max(w => w.Id) + 1;
        }
    }
}
