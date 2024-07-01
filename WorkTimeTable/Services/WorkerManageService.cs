using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
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
using WorkTimeTable.Controls;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Converters;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Messages;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.ViewModels;

namespace WorkTimeTable.Services
{
    public interface IWorkerManageService
    {
        IReadOnlyCollection<IWorker>? LastLoadedWorkers { get; }
        IWorker? TargetWorker { get; set; }

        void InitializeFilter(int year, int month);
        bool IsExistWorker(string name, string birthDate);
        Task<IEnumerable<IWorker>?> LoadWorkersAsync();
        int NextNewWorkerId(bool isFillBlank = true);
        Task<bool> SaveWorkersAsync();
        bool TryAddWorker(string name, string birthDate, string colorName, DayOfWeekFlag? fixedWorkWeeks, out IWorker? newWorker);
        bool TryAddWorker(IWorker newWorker);
        bool TryRemoveWorker(int id, out IWorker? removedWorker);
    }

    internal class WorkerManageService : IWorkerManageService
    {
        const string WorkerListPathKey = "WorkerListPath";

        readonly IConfiguration _configuration;
        readonly ILogger _logger;

        IWorker? _TargetWorker = null;
        IList<IWorker>? _lastLoadedWorkers = null;

        public IWorker? TargetWorker
        {
            get => _TargetWorker;
            set
            {
                if (_TargetWorker != value)
                {
                    _TargetWorker = value;
                    onTargetWorkerChanged();
                }
            }
        }
        public IReadOnlyCollection<IWorker>? LastLoadedWorkers => _lastLoadedWorkers?.AsReadOnly();

        public WorkerManageService(ILogger<WorkerManageService> logger, IConfiguration config)
        {
            _configuration = config;
            _logger = logger;
        }

        public async Task<IEnumerable<IWorker>?> LoadWorkersAsync()
        {
            string? listPath = _configuration.GetValue<string>(WorkerListPathKey);
            if (String.IsNullOrEmpty(listPath))
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

            _logger.LogInformation($"Reading workers from file: {listPath}");

            string jsonStr = await File.ReadAllTextAsync(listPath);
            if (String.IsNullOrEmpty(jsonStr))
            {
                _logger.LogWarning($"EmptyFile: {nameof(listPath)}");
                return null;
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.ReadCommentHandling = JsonCommentHandling.Skip;
            options.Converters.Add(new WorkerModelJsonConverter());
            options.Converters.Add(new WorkTimeModelJsonConverter());

            try
            {
                _lastLoadedWorkers = JsonSerializer.Deserialize<WorkerModel[]>(jsonStr, options)?.Cast<IWorker>().ToList();
                if (_lastLoadedWorkers != null)
                {
                    _logger.LogInformation($"{_lastLoadedWorkers.Count} workers loaded");
                    WeakReferenceMessenger.Default.Send(new WorkerListLoadedMessage(new WorkerListLoadedMessageArgs(_lastLoadedWorkers)));
                }

                return _lastLoadedWorkers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deserialize Error");
                return null;
            }
        }
        public void InitializeFilter(int year, int month)
        {
            if(year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
                throw new ArgumentOutOfRangeException(nameof(year));

            if(month < 1 || month > 12)
                throw new ArgumentOutOfRangeException(nameof(month));

            WeakReferenceMessenger.Default.Send<WorkTimeFilterChangedMessage>(new WorkTimeFilterChangedMessage(new WorkTimeFilter(year, month)));
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
            options.Converters.Add(new WorkerModelJsonConverter());
            options.Converters.Add(new WorkTimeModelJsonConverter());
            options.WriteIndented = true;

            try
            {
                string jsonWorkersStr = JsonSerializer.Serialize<IEnumerable<WorkerModel>>(_lastLoadedWorkers.Cast<WorkerModel>(), options);
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
        public bool TryAddWorker(string name, string birthDate, string colorName, DayOfWeekFlag? fixedWorkWeeks, out IWorker? newWorker)
        {
            if (_lastLoadedWorkers is null)
                throw new NullReferenceException(nameof(_lastLoadedWorkers));
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            if (String.IsNullOrEmpty(birthDate))
                throw new ArgumentNullException(nameof(birthDate));
            if(String.IsNullOrEmpty(colorName))
                throw new ArgumentNullException(nameof(colorName));

            newWorker = null;

            int newId = NextNewWorkerId();
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

            newWorker = new WorkerModel { Id = newId, Name = name, BirthDate = birthDate, ColorName = colorName, FixedWorkWeeks = fixedWorkWeeks.Value };

            _lastLoadedWorkers.Add(newWorker);

            _logger.LogInformation($"Added new worker: {newWorker}");
            WeakReferenceMessenger.Default.Send(new WorkerListChangedMessage(new WorkerListChangedMessageArgs(WorkerListChangedStatus.Added, new IWorker[] { newWorker })));

            return true;
        }
        public bool TryAddWorker(IWorker newWorker)
        {
            if (newWorker is null)
                throw new ArgumentNullException(nameof(newWorker));

            if (_lastLoadedWorkers is null)
                throw new NullReferenceException(nameof(_lastLoadedWorkers));

            if (IsExistWorker(newWorker.Name, newWorker.BirthDate))
            {
                _logger.LogWarning($"AlreadyExist: {newWorker.Name}, {newWorker.BirthDate}");
                return false;
            }

            bool isExistId = _lastLoadedWorkers.Any(w => w.Id == newWorker.Id);
            if(isExistId)
            {
                _logger.LogWarning($"AlreadyExist: {newWorker.Id}");
                return false;
            }

            _lastLoadedWorkers.Add(newWorker);

            _logger.LogInformation($"Added new worker: {newWorker}");
            WeakReferenceMessenger.Default.Send(new WorkerListChangedMessage(new WorkerListChangedMessageArgs(WorkerListChangedStatus.Added, new IWorker[] { newWorker })));

            return true;
        }
        public bool TryRemoveWorker(int id, out IWorker? removedWorker)
        {
            removedWorker = null;

            if (_lastLoadedWorkers is null)
                throw new NullReferenceException(nameof(_lastLoadedWorkers));
            if(id < 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            removedWorker = _lastLoadedWorkers.FirstOrDefault(w => w.Id == id);
            if(removedWorker is null)
            {
                _logger.LogWarning($"NotExist: {id}");
                return false;
            }

            _lastLoadedWorkers.Remove(removedWorker);
            _logger.LogInformation($"Removed new worker: {removedWorker}");

            WeakReferenceMessenger.Default.Send(new WorkerListChangedMessage(new WorkerListChangedMessageArgs(WorkerListChangedStatus.Removed, new IWorker[] { removedWorker })));

            return true;
        }

        private void onTargetWorkerChanged()
        {
            WeakReferenceMessenger.Default.Send(new TargetWorkerChangedMessage(_TargetWorker));
        }

        public int NextNewWorkerId(bool isFillBlank = true)
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
