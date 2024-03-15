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
    public partial class WorkTimeModel : ObservableObject
    {
        [ObservableProperty]
        int _WorkerId;

        [JsonIgnore]
        [ObservableProperty]
        WorkTimeType _WorkTimeType;

        [JsonIgnore]
        [ObservableProperty]
        int _Year;

        [JsonIgnore]
        [ObservableProperty]
        int _Month;

        [JsonIgnore]
        [ObservableProperty]
        int _Day;

        [JsonIgnore]
        [ObservableProperty]
        int _Hour;

        [JsonIgnore]
        [ObservableProperty]
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
