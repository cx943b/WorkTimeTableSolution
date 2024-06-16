namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface IFixedWorker
    {
        DayOfWeekFlag FixedWorkWeeks { get; set; }
        bool IsWorkDay(DayOfWeekFlag targetWeek) => FixedWorkWeeks.HasFlag(targetWeek);
        bool IsWorkDay(DateTime date) => IsWorkDay((DayOfWeekFlag)date.DayOfWeek);
    }
}
