namespace WorkTimeTable.Infrastructure
{
    [Flags]
    public enum DayOfWeekFlag
    {
        Sunday = 0x0000,
        Monday = 0x0001,
        Tuesday = 0x0010,
        Wednesday = 0x0011,
        Thursday = 0x0100,
        Friday = 0x0101,
        Saturday = 0x0110
    }
}
