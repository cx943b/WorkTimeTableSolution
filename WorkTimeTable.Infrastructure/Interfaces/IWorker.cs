using System.Windows.Media;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface  IWorker
    {
        int Id { get; init; }
        string Name { get; set; }
        SolidColorBrush Brush { get; set; }

        DayOfWeekFlag FixedWorkWeeks { get; set; }

        IReadOnlyCollection<WorkTimeModel> WorkTimes { get; }

        bool IsWorkDay(DayOfWeekFlag targetWeek) => FixedWorkWeeks.HasFlag(targetWeek);
        bool IsWorkDay(DateTime date) => IsWorkDay((DayOfWeekFlag)date.DayOfWeek);
    }
}
