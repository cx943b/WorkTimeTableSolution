using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Tests
{
    [TestClass]
    public class WorkTimeTests : WorkerTestBase
    {
        [TestMethod]
        public void WorkTimeFilterTest()
        {
            var targetWorker = Workers.First();
            targetWorker.ApplyWorkTimeFilter(new WorkTimeFilter(2024, 2));

            foreach (var workTime in targetWorker.FilteredWorkTimes.Cast<WorkTimeModel>())
                Debug.WriteLine($"{workTime.Month} {workTime.Day}");
        }

    }
}
