namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface IWorkTime
    {
        int WorkerId { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
    }
}
