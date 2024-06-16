using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkTimeTable.Infrastructure
{
    [Flags]
    public enum DayOfWeekFlag
    {
        None =      0x0000_0001,

        [Description("Sun")]
        Sunday =    0x0000_0010,
        [Description("Mon")]
        Monday =    0x0000_0100,
        [Description("Tue")]
        Tuesday =   0x0000_1000,
        [Description("Wed")]
        Wednesday = 0x0001_0000,
        [Description("Thu")]
        Thursday =  0x0010_0000,
        [Description("Fri")]
        Friday =    0x0100_0000,
        [Description("Sat")]
        Saturday =  0x1000_0000
    }

    public enum WorkTimeType
    {
        Fixed,
        Alter
    }
}
