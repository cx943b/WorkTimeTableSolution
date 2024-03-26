using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WorkTimeTable.Controls
{
    public class WorkTimeItemsControl : ItemsControl
    {
        static readonly int[] _Months = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        static readonly int[] _Days = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        static readonly int[] _Hours = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        static readonly int[] _Minutes = new int[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

        public IEnumerable<int> Months => _Months;
        public IEnumerable<int> Days => _Days;
        public IEnumerable<int> Hours => _Hours;
        public IEnumerable<int> Minutes => _Minutes;
    }
}
