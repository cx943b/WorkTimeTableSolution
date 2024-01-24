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
        string _Name;

        [JsonConverter(typeof(SolidColorBrushJsonConverter))]
        [ObservableProperty]
        SolidColorBrush _Brush;

        [ObservableProperty]
        DayOfWeekFlag _FixedWorkWeeks;

        readonly ObservableCollection<IWorkTime> _WorkTimes;

        public IReadOnlyCollection<IWorkTime> WorkTimes => _WorkTimes;

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
        public WorkerModel(int id, string name, SolidColorBrush brush, IReadOnlyCollection<IWorkTime>? workTimes = null) : this(id, name, brush)
        {
            if (workTimes != null && workTimes.Any())
                _WorkTimes = new(workTimes);
        }

        [JsonConstructor]
        public WorkerModel(int id, string name, SolidColorBrush brush, DayOfWeekFlag fixedWorkWeeks, IReadOnlyCollection<IWorkTime>? workTimes = null) : this(id, name, brush, workTimes)
        {
            _FixedWorkWeeks = fixedWorkWeeks;
        }

        public override string ToString() => $"{Id}: {Name}";
    }
}
