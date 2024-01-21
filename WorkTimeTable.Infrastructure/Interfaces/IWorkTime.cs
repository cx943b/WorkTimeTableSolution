namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface IWorkTime
    {
        int WorkerId { get; init; }
        DateTime StartTime { get; set; }
        TimeSpan WorkTimeSpan { get; set; }

        DateTime EndTime { get; }
    }
}
