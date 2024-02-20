using Castle.Core.Configuration;
using Castle.Core.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;

namespace WorkTimeTable.Tests
{
    [TestClass]
    public class WorkerManageServiceTests : TestBase
    {
        [TestMethod]
        public async Task AddFailByBirthDateNewWorker()
        {
            var logger = CreateLogger<WorkerManageService>();
            var config = GetConfig();

            WorkerManageService workerMgrSvc = new WorkerManageService(logger, config);
            await workerMgrSvc.LoadWorkersAsync();

            bool isAdded = workerMgrSvc.TryAddWorker("ABC", "232323", new SolidColorBrush(Colors.CornflowerBlue), Infrastructure.DayOfWeekFlag.Monday, out WorkerModel? newWorker);
            Assert.IsFalse(isAdded);
        }

        [TestMethod]
        public async Task AddFailByExistNewWorker()
        {
            var logger = CreateLogger<WorkerManageService>();
            var config = GetConfig();

            WorkerManageService workerMgrSvc = new WorkerManageService(logger, config);
            await workerMgrSvc.LoadWorkersAsync();

            bool isAdded = workerMgrSvc.TryAddWorker("AAA", "121212", new SolidColorBrush(Colors.CornflowerBlue), Infrastructure.DayOfWeekFlag.Monday, out WorkerModel? newWorker);
            Assert.IsFalse(isAdded);
        }

        [TestMethod]
        public async Task AddNewWorker()
        {
            var logger = CreateLogger<WorkerManageService>();
            var config = GetConfig();

            WorkerManageService workerMgrSvc = new WorkerManageService(logger, config);
            await workerMgrSvc.LoadWorkersAsync();

            bool isAdded = workerMgrSvc.TryAddWorker("ABC", "121212", new SolidColorBrush(Colors.CornflowerBlue), Infrastructure.DayOfWeekFlag.Monday, out WorkerModel? newWorker);
            Assert.IsTrue(isAdded);
        }
    }
}
