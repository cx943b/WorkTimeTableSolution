using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable.ViewModels
{
    public partial class YearMonthFilterViewModel :ObservableObject
    {
        [ObservableProperty]
        int _TargetYear = DateTime.Now.Year;

        [ObservableProperty]
        int _TargetMonth = 1; // DateTime.Now.Month;

        [ObservableProperty]
        IEnumerable<int> _TargetMonths = Enumerable.Range(1, 12);
    }
}
