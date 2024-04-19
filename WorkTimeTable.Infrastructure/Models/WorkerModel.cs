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

namespace WorkTimeTable.Infrastructure.Models
{
    //[JsonConverter(typeof(WorkerModelJsonConverter))]
    [JsonDerivedType(typeof(WorkerModel), typeDiscriminator: "Worker")]
    [JsonDerivedType(typeof(FixedWorkerModel), typeDiscriminator: "FixedWorker")]
    public partial class WorkerModel : ObservableObject, IEqualityComparer<WorkerModel>, IWorker
    {
        [ObservableProperty]
        int _Id = 0;
        
        [ObservableProperty]
        string _Name = "";

        [ObservableProperty]
        string _BirthDate = "000000";

        //[JsonConverter(typeof(SolidColorBrushJsonConverter))]
        [ObservableProperty]
        string _ColorName = nameof(Colors.CornflowerBlue);

        [ObservableProperty]
        List<WorkTimeModel> _WorkTimes = new List<WorkTimeModel>();
        
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
