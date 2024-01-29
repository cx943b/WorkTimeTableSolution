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

namespace WorkTimeTable.Infrastructure.Models
{
    [JsonConverter(typeof(WorkerModelJsonConverter))]
    public partial class WorkerModel : ObservableObject, IWorker
    {
        public int Id { get; init; }
        
        [ObservableProperty]
        string _Name = "";

        [ObservableProperty]
        string _BirthDate = "000000";

        [JsonConverter(typeof(SolidColorBrushJsonConverter))]
        [ObservableProperty]
        SolidColorBrush _Brush = Brushes.CornflowerBlue;

        [ObservableProperty]
        DayOfWeekFlag _FixedWorkWeeks = DayOfWeekFlag.None;

        readonly ObservableCollection<WorkTimeModel> _WorkTimes;
        public IReadOnlyCollection<WorkTimeModel> WorkTimes => _WorkTimes;

        // For Converter
        internal WorkerModel()
        {
            _WorkTimes = new();
        }

        public WorkerModel(int id, string name)
        {
            if(id < 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name), "Name must not be null or empty");

            Id = id;
            Name = name;
            Brush = Brushes.CornflowerBlue;

            _WorkTimes = new();
        }
        public WorkerModel(int id, string name, SolidColorBrush brush) : this(id, name) => Brush = brush;
        public WorkerModel(int id, string name, SolidColorBrush brush, IReadOnlyCollection<WorkTimeModel>? workTimes = null) : this(id, name, brush)
        {
            if (workTimes != null && workTimes.Any())
                _WorkTimes = new(workTimes);
        }

        //[JsonConstructor]
        public WorkerModel(int id, string name, SolidColorBrush brush, DayOfWeekFlag fixedWorkWeeks, IReadOnlyCollection<WorkTimeModel>? workTimes = null) : this(id, name, brush, workTimes)
        {
            _FixedWorkWeeks = fixedWorkWeeks;
        }

        public void AddWorkTime(WorkTimeModel workTime)
        {
            if(workTime == null)
                throw new ArgumentNullException(nameof(workTime), "WorkTime must not be null");

            _WorkTimes.Add(workTime);
        }
        public void AddWorkTime(IEnumerable<WorkTimeModel> workTimes)
        {
            if (workTimes == null)
                throw new ArgumentNullException(nameof(workTimes), "WorkTimes must not be null");

            foreach (var workTime in workTimes)
            {
                if (workTime == null)
                    throw new NullReferenceException($"{nameof(WorkTimeModel)} must not be null");

                _WorkTimes.Add(workTime);
            }
        }
        public void RemoveWorkTime(WorkTimeModel workTime)
        {
            if (workTime == null)
                throw new ArgumentNullException(nameof(workTime), "WorkTime must not be null");

            _WorkTimes.Remove(workTime);
        }
        public void RemoveWorkTime(IEnumerable<WorkTimeModel> workTimes)
        {
            if (workTimes == null)
                throw new ArgumentNullException(nameof(workTimes), "WorkTimes must not be null");

            foreach (var workTime in workTimes)
            {
                if (workTime == null)
                    throw new NullReferenceException($"{nameof(WorkTimeModel)} must not be null");

                _WorkTimes.Remove(workTime);
            }
        }


        public override string ToString() => $"{Id}: {Name}";
    }
}
