using System.Windows.Media;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WorkTimeTable.Infrastructure.Models
{
    public partial class FilteredWorkTimesModel : ObservableObject
    {
        public int WorkerId { get; init; }
        public int Year { get; init; } = DateTime.Now.Year;
        public int Month { get; init; } = DateTime.Now.Month;

        [ObservableProperty]
        string _ColorName = nameof(Colors.CornflowerBlue);

        public ObservableCollection<WorkTimeModel> WorkTimes { get; init; } = new ObservableCollection<WorkTimeModel>();

        public FilteredWorkTimesModel(int workerId, int year, int month, string colorName, IEnumerable<WorkTimeModel>? workTimes = null)
        {
            WorkerId = workerId;
            Year = year;
            Month = month;
            ColorName = colorName;

            if (workTimes != null)
            {
                foreach (var workTime in workTimes)
                    WorkTimes.Add(workTime);
            }
        }

        public void AddWorkTime(WorkTimeModel workTime)
        {
            if(WorkerId == workTime.WorkerId && workTime.Year == Year && workTime.Month == Month)
            {
                WorkTimes.Add(workTime);
            }
            else
            {
                throw new InvalidOperationException("WorkTime is not belong to this filter");
            }
        }
        public bool RemoveWorkTime(WorkTimeModel workTime) => WorkTimes.Remove(workTime);
    }
}
