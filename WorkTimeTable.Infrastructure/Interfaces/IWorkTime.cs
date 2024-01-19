namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface IWorkTime
    {
        int WorkerId { get; set; }
        DateTime StartTime { get; set; }
        TimeSpan WorkTime { get; set; }

        DateTime EndTime { get; }
    }
}
