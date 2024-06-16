namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface IWorkTime
    {
        int WorkerId { get; set; }
        DateTime StartWorkTime { get; }
        WorkTimeType WorkTimeType { get; set; }
        DateTime EndWorkTime { get; }
    }
}
