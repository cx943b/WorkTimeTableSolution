using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure.Interfaces;
using System.Collections.ObjectModel;

namespace WorkTimeTable.Infrastructure.Models
{
    public partial class WorkerModel : ObservableObject, IWorker
    {
        public int Id { get; init; }
        
        [ObservableProperty]
        string _Name;

        [ObservableProperty]
        Color _Color;

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

            _WorkTimes = new();
        }
        public WorkerModel(int id, string name, Color color) : this(id, name) => Color = color;
        public WorkerModel(int id, string name, Color color, IEnumerable<IWorkTime> workTimes) : this(id, name, color)
        {
            if(workTimes == null)
                throw new ArgumentNullException(nameof(workTimes), "WorkTimes must not be null");
            if(!workTimes.Any())
                throw new ArgumentException("WorkTimes must not be empty");

            _WorkTimes = new(workTimes);
        }
    }
}
