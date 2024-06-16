using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface  IWorker
    {
        int Id { get; set; }
        string Name { get; set; }
        string ColorName { get; set; }
        string BirthDate { get; set; }
        IEnumerable<WorkTimeModel> WorkTimes { get; }

        void AddWorkTime(WorkTimeModel workTime);
        void RemoveWorkTime(WorkTimeModel workTime);
        void RemoveWorkTimes(int year);
        void RemoveWorkTimes(int year, int month);
        FilteredWorkTimesModel GetFilteredWorkTimes(int year, int month);
    }
}
