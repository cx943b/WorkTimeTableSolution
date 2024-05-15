﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure.Interfaces;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Documents;
using WorkTimeTable.Infrastructure.Converters;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Data;
using System.Windows.Markup;
using System.ComponentModel;

namespace WorkTimeTable.Infrastructure.Models
{
    public class FilteredWorkTimesModel
    {
        public int WorkerId { get; init; }
        public int Year { get; init; } = DateTime.Now.Year;
        public int Month { get; init; } = DateTime.Now.Month;
        public string ColorName { get; set; } = nameof(Colors.CornflowerBlue);

        public ObservableCollection<WorkTimeModel> WorkTimes { get; init; } = new ObservableCollection<WorkTimeModel>();

        public FilteredWorkTimesModel(int workerId, int year, int month, string colorName, IEnumerable<WorkTimeModel>? workTimes = null)
        {
            WorkerId = workerId;
            Year = year;
            Month = month;
            ColorName = colorName;

            if (workTimes != null)
            {
                foreach (var workTime in workTimes)
                {
                    WorkTimes.Add(workTime);
                }
            }
        }

        public void AddWorkTime(WorkTimeModel workTime)
        {
            if(WorkerId == workTime.WorkerId && workTime.Year == Year && workTime.Month == Month)
            {
                WorkTimes.Add(workTime);
            }
            else
            {
                throw new InvalidOperationException("WorkTime is not belong to this filter");
            }
        }
        public bool RemoveWorkTime(WorkTimeModel workTime) => WorkTimes.Remove(workTime);
    }


    //[JsonConverter(typeof(WorkerModelJsonConverter))]
    [JsonDerivedType(typeof(WorkerModel), typeDiscriminator: "Worker")]
    [JsonDerivedType(typeof(FixedWorkerModel), typeDiscriminator: "FixedWorker")]
    public partial class WorkerModel : ObservableObject, IEqualityComparer<WorkerModel>, IWorker
    {
        // Year, Month, WorkTimes
        readonly Dictionary<int, Dictionary<int, List<WorkTimeModel>>> _dicWorkTimes = new Dictionary<int, Dictionary<int, List<WorkTimeModel>>>();
        WorkTimeFilter? _currentFilter;

        [ObservableProperty]
        int _Id = 0;
        
        [ObservableProperty]
        string _Name = "";

        [ObservableProperty]
        string _BirthDate = "000000";

        //[JsonConverter(typeof(SolidColorBrushJsonConverter))]
        [ObservableProperty]
        string _ColorName = nameof(Colors.CornflowerBlue);



        public void AddWorkTime(WorkTimeModel workTime)
        {
            if (_dicWorkTimes.TryGetValue(workTime.Year, out var dicWorktimesByYear))
            {
                if (dicWorktimesByYear.TryGetValue(workTime.Month, out var workTimesByMonth))
                {
                    workTimesByMonth.Add(workTime);
                }
                else
                {
                    dicWorktimesByYear[workTime.Month] = new List<WorkTimeModel> { workTime };
                }
            }
            else
            {
                _dicWorkTimes[workTime.Year] = new Dictionary<int, List<WorkTimeModel>>
                {
                    [workTime.Month] = new List<WorkTimeModel> { workTime }
                };
            }
        }
        public void RemoveWorkTime(WorkTimeModel workTime)
        {
            if (_dicWorkTimes.TryGetValue(workTime.Year, out var monthDic))
            {
                if (monthDic.TryGetValue(workTime.Month, out var workTimeList))
                {
                    workTimeList.Remove(workTime);
                }
            }
        }

        public FilteredWorkTimesModel? TryGetFilteredWorkTimes(int year, int month)
        {
            if (_dicWorkTimes.TryGetValue(year, out var dicWorktimesByYear))
            {
                if (dicWorktimesByYear.TryGetValue(month, out var workTimesByMonth))
                {
                    return new FilteredWorkTimesModel(Id, year, month, ColorName, workTimesByMonth);
                }
            }

            return null;
        }












        public ObservableCollection<WorkTimeModel> WorkTimes { get; init; } = new ObservableCollection<WorkTimeModel>();

        [ObservableProperty]
        [property: JsonIgnore]
        [property: ReadOnly]
        IEnumerable<WorkTimeModel>? _FilteredWorkTimes;

        public void ApplyWorkTimeFilter(WorkTimeFilter filter)
        {
            if(_currentFilter != null && filter == _currentFilter)
                return;

            _currentFilter = null;
            FilteredWorkTimes = null;

            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            _currentFilter = filter;

            FilteredWorkTimes = WorkTimes
                .Where(workTime => workTime.Month == filter.Month && workTime.Year == filter.Year)
                .OrderBy(workTime => workTime.Day)
                .ToArray();
        }
        public void ClearWorkTimeFilter()
        {
            _currentFilter = null;
            FilteredWorkTimes = null;
        }

        public override string ToString() => $"{Id}: {Name}";
        public int GetHashCode([DisallowNull] WorkerModel obj) => obj.Id;
        public bool Equals(WorkerModel? x, WorkerModel? y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id;
        }
    }

    public partial class FixedWorkerModel : WorkerModel, IFixedWorker
    {
        [ObservableProperty]
        DayOfWeekFlag _FixedWorkWeeks = DayOfWeekFlag.None;
    }
}
