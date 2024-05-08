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

namespace WorkTimeTable.Infrastructure.Models
{
    //[JsonConverter(typeof(WorkerModelJsonConverter))]
    [JsonDerivedType(typeof(WorkerModel), typeDiscriminator: "Worker")]
    [JsonDerivedType(typeof(FixedWorkerModel), typeDiscriminator: "FixedWorker")]
    public partial class WorkerModel : ObservableObject, IEqualityComparer<WorkerModel>, IWorker
    {
        WorkTimeFilter? _currentFilter;

        readonly CollectionViewSource _cvs = new CollectionViewSource();

        [ObservableProperty]
        int _Id = 0;
        
        [ObservableProperty]
        string _Name = "";

        [ObservableProperty]
        string _BirthDate = "000000";

        //[JsonConverter(typeof(SolidColorBrushJsonConverter))]
        [ObservableProperty]
        string _ColorName = nameof(Colors.CornflowerBlue);

        public List<WorkTimeModel> WorkTimes { get; set; } = new List<WorkTimeModel>();

        [JsonIgnore]
        public ICollectionView FilteredWorkTimes => _cvs.View;

        public WorkerModel()
        {
            _cvs.Source = WorkTimes;
            _cvs.SortDescriptions.Add(new SortDescription(nameof(WorkTimeModel.Day), ListSortDirection.Ascending));
            _cvs.IsLiveFilteringRequested = true;
            _cvs.IsLiveSortingRequested = true;
        }

        public void ApplyWorkTimeFilter(WorkTimeFilter filter)
        {
            _currentFilter = null;
            _cvs.Filter -= workTimeFilterHandler;

            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            _currentFilter = filter;
            _cvs.Filter += workTimeFilterHandler;

            //var filteredWorkTimes = _WorkTimes
            //    .Where(workTime => workTime.Month == filter.Month && workTime.Year == filter.Year)
            //    .OrderBy(workTime => workTime.Day);

            //foreach (var workTime in filteredWorkTimes)
            //    FilteredWorkTimes.Add(workTime);
        }
        
        private void workTimeFilterHandler(object? sender, FilterEventArgs e)
        {
            // sender is CollectionViewSource
            // If executed this Hanler, _currentFilter is not null

            var workTime = (WorkTimeModel)e.Item;
            e.Accepted = workTime.Year == _currentFilter!.Year && workTime.Month == _currentFilter.Month;
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
