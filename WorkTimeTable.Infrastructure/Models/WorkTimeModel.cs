using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure.Interfaces;

namespace WorkTimeTable.Infrastructure.Models
{
    // https://github.com/CommunityToolkit/dotnet/issues/413
    public partial class WorkTimeModel : ObservableObject
    {
        [ObservableProperty]
        int _WorkerId;

        [ObservableProperty]
        WorkTimeType _WorkTimeType;

        [ObservableProperty]
        [property: JsonIgnore]
        int _Year;

        [ObservableProperty]
        [property: JsonIgnore]
        int _Month;

        [ObservableProperty]
        [property: JsonIgnore]
        int _Day;

        [ObservableProperty]
        [property: JsonIgnore]
        int _Hour;

        [ObservableProperty]
        [property: JsonIgnore]
        int _Minute;

        [ObservableProperty]
        TimeSpan _WorkTimeSpan;

        public DateTime StartWorkTime => new DateTime(Year, Month, Day, Hour, Minute, 0);
        [JsonIgnore]
        public DateTime EndWorkTime => StartWorkTime.Add(WorkTimeSpan);


        public WorkTimeModel() { }
        public WorkTimeModel(int workerId)
        {
            WorkerId = workerId;
        }
    }
}
