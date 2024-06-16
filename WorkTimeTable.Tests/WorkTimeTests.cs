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
            var filteredWorkTimes = targetWorker.GetFilteredWorkTimes(2024, 1);
            
            Assert.IsNotNull(filteredWorkTimes);
        }
    }
}