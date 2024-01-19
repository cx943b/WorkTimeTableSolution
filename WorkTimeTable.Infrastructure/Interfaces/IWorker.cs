using System.Windows.Media;

namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface  IWorker
    {
        int Id { get; init; }
        string Name { get; set; }
        Brush Brush { get; set; }

        DayOfWeekFlag FixedWorkWeeks { get; set; }

        IReadOnlyCollection<IWorkTime> WorkTimes { get; }

        bool IsWorkDay(DayOfWeekFlag targetWeek) => FixedWorkWeeks.HasFlag(targetWeek);
        bool IsWorkDay(DateTime date) => IsWorkDay((DayOfWeekFlag)date.DayOfWeek);
    }
}
