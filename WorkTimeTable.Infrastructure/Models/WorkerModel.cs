using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Text.Json;

namespace WorkTimeTable.Infrastructure.Models
{
    //[JsonConverter(typeof(WorkerModelJsonConverter))]
    //[JsonDerivedType(typeof(WorkerModel), typeDiscriminator: "Worker")]
    //[JsonDerivedType(typeof(FixedWorkerModel), typeDiscriminator: "FixedWorker")]
    public partial class WorkerModel : ObservableObject, IEqualityComparer<WorkerModel>, IWorker
    {
        // Key: Year, Month, WorkTimes
        readonly Dictionary<int, Dictionary<int, List<WorkTimeModel>>> _dicWorkTimes = new Dictionary<int, Dictionary<int, List<WorkTimeModel>>>();

        [ObservableProperty]
        int _Id = 0;
        
        [ObservableProperty]
        string _Name = "";

        [ObservableProperty]
        string _BirthDate = "000000";

        [ObservableProperty]
        string _ColorName = nameof(Colors.CornflowerBlue);

        [JsonIgnore]
        public IEnumerable<WorkTimeModel> WorkTimes => _dicWorkTimes.Values.SelectMany(x => x.Values.SelectMany(y => y));

        public void AddWorkTime(WorkTimeModel workTime)
        {
            workTime.WorkerId = Id;

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
            if (!_dicWorkTimes.TryGetValue(workTime.Year, out var monthDic) || !monthDic.TryGetValue(workTime.Month, out var workTimeList))
                return;

            workTimeList.Remove(workTime);
        }
        public void RemoveWorkTimes(int year)
        {
            if(_dicWorkTimes.ContainsKey(year))
                _dicWorkTimes.Remove(year);
        }
        public void RemoveWorkTimes(int year, int month)
        {
            if (!_dicWorkTimes.TryGetValue(year, out var workTimesByYear) || !workTimesByYear.ContainsKey(month))
                return;

            workTimesByYear.Remove(month);
        }

        // Return Empty WorkTimesModel if not found
        public FilteredWorkTimesModel GetFilteredWorkTimes(int year, int month)
        {
            if (_dicWorkTimes.TryGetValue(year, out var dicWorktimesByYear) && dicWorktimesByYear.TryGetValue(month, out var workTimesByMonth))
                return new FilteredWorkTimesModel(Id, year, month, ColorName, workTimesByMonth);

            return new FilteredWorkTimesModel(Id, year, month, ColorName);
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
