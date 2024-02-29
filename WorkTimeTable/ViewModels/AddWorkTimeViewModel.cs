using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable.ViewModels
{
    public partial class AddWorkTimeViewModel : SosoMessageBoxViewModelBase
    {
        [ObservableProperty]
        int _Year = DateTime.Now.Year;

        [ObservableProperty]
        int _Month = DateTime.Now.Month;

        [ObservableProperty]
        int _Day = DateTime.Now.Day;

        [ObservableProperty]
        int _Hour = DateTime.Now.Hour;

        [ObservableProperty]
        int _Minute = DateTime.Now.Minute;
    }
}
