namespace WorkTimeTable.Infrastructure
{
    public class WorkTimeFilter
    {
        public int Year { get; init; }
        public int Month { get; init; }

        public WorkTimeFilter(int year, int month) => (Year, Month) = (year, month);
    }
}
