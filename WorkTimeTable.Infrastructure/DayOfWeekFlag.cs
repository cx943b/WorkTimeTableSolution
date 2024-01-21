namespace WorkTimeTable.Infrastructure
{
    [Flags]
    public enum DayOfWeekFlag
    {
        Sunday =    0x0000_0000,
        Monday =    0x0000_0001,
        Tuesday =   0x0000_0010,
        Wednesday = 0x0000_0100,
        Thursday =  0x0000_1000,
        Friday =    0x0001_0000,
        Saturday =  0x0010_0000
    }
}
