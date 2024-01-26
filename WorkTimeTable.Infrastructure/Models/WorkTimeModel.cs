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
        public int WorkerId { get; init; }

        [ObservableProperty]
        WorkTimeType _WorkTimeType;

        [ObservableProperty]
        DateTime _StartTime;

        [ObservableProperty]
        TimeSpan _WorkTimeSpan;

        [JsonIgnore]
        public DateTime EndTime { get; private set; }

        [JsonConstructor]
        public WorkTimeModel(int workerId)
        {
            WorkerId = workerId;
        }
    }
}
