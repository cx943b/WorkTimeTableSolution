namespace WorkTimeTable.Infrastructure
{
    [Flags]
    public enum DayOfWeekFlag
    {
        None =      0x0000_0000,
        Sunday =    0x0000_0010,
        Monday =    0x0000_0100,
        Tuesday =   0x0000_1000,
        Wednesday = 0x0001_0000,
        Thursday =  0x0010_0000,
        Friday =    0x0100_0000,
        Saturday =  0x1000_0000
    }

    public enum WorkTimeType
    {
        Fixed,
        Alter
    }
}
