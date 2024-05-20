using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure.Converters;
using WorkTimeTable.Infrastructure.Interfaces;

namespace WorkTimeTable.Infrastructure.Models
{
    // https://github.com/CommunityToolkit/dotnet/issues/413
    [JsonConverter(typeof(WorkTimeModelJsonConverter))]
    public partial class WorkTimeModel : ObservableValidator, IEquatable<WorkTimeModel>, IWorkTime
    {
        [ObservableProperty]
        int _WorkerId;

        [ObservableProperty]
        WorkTimeType _WorkTimeType;

        [ObservableProperty]
        [Range(1, 9999)]
        [NotifyPropertyChangedFor(nameof(StartWorkTime))]
        [NotifyPropertyChangedFor(nameof(EndWorkTime))]
        [property: JsonIgnore]
        int _Year;

        [ObservableProperty]
        [Range(1, 12)]
        [NotifyPropertyChangedFor(nameof(StartWorkTime))]
        [NotifyPropertyChangedFor(nameof(EndWorkTime))]
        [property: JsonIgnore]
        int _Month;

        [ObservableProperty]
        [Range(1, 31)]
        [NotifyPropertyChangedFor(nameof(StartWorkTime))]
        [NotifyPropertyChangedFor(nameof(EndWorkTime))]
        [property: JsonIgnore]
        int _Day;

        [ObservableProperty]
        [Range(0, 23)]
        [NotifyPropertyChangedFor(nameof(StartWorkTime))]
        [NotifyPropertyChangedFor(nameof(EndWorkTime))]
        [property: JsonIgnore]
        int _Hour;

        [ObservableProperty]
        [Range(0, 59)]
        [NotifyPropertyChangedFor(nameof(StartWorkTime))]
        [NotifyPropertyChangedFor(nameof(EndWorkTime))]
        [property: JsonIgnore]
        int _Minute;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(EndWorkTime))]
        TimeSpan _WorkTimeSpan;

        public DateTime StartWorkTime => new DateTime(Year, Month, Day, Hour, Minute, 0);
        [JsonIgnore]
        public DateTime EndWorkTime => StartWorkTime.Add(WorkTimeSpan);

        public override int GetHashCode() => StartWorkTime.GetHashCode() ^ EndWorkTime.GetHashCode();
        public override bool Equals(object? obj) => this.Equals(obj as WorkTimeModel);
        public bool Equals(WorkTimeModel? other)
        {
            if (other is null)
                return false;

            return StartWorkTime == other.StartWorkTime && EndWorkTime == other.EndWorkTime;
        }
    }
}
