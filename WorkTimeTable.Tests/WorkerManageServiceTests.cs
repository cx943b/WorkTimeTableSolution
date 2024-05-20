using Castle.Core.Configuration;
using Castle.Core.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WorkTimeTable.Infrastructure.Models;
using WorkTimeTable.Services;

namespace WorkTimeTable.Tests
{
    [TestClass]
    public class WorkerManageServiceTests : MvvmTestBase
    {
        [TestMethod]
        public void GetWorkerColors()
        {
            PropertyInfo[] propInfos = typeof(Colors).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var dicColors = propInfos.Select(pi => new KeyValuePair<string, Color>(pi.Name, (Color)ColorConverter.ConvertFromString(pi.Name))).ToDictionary();

            Debug.WriteLine(propInfos);
        }

        [TestMethod]
        public async Task LoadWorkers()
        {
            var logger = CreateLogger<WorkerManageService>();
            var config = GetConfig();

            WorkerManageService workerMgrSvc = new WorkerManageService(logger, config);
            await workerMgrSvc.LoadWorkersAsync();

            Assert.IsNotNull(workerMgrSvc.LastLoadedWorkers);
            Assert.IsTrue(workerMgrSvc.LastLoadedWorkers.Count > 0);
        }
        

        [TestMethod]
        public async Task AddFailByBirthDateNewWorker()
        {
            var logger = CreateLogger<WorkerManageService>();
            var config = GetConfig();

            WorkerManageService workerMgrSvc = new WorkerManageService(logger, config);
            await workerMgrSvc.LoadWorkersAsync();

            bool isAdded = workerMgrSvc.TryAddWorker("ABC", "232323", nameof(Colors.CornflowerBlue), Infrastructure.DayOfWeekFlag.Monday, out WorkerModel? newWorker);
            Assert.IsFalse(isAdded);
        }

        [TestMethod]
        public async Task AddFailByExistNewWorker()
        {
            var logger = CreateLogger<WorkerManageService>();
            var config = GetConfig();

            WorkerManageService workerMgrSvc = new WorkerManageService(logger, config);
            await workerMgrSvc.LoadWorkersAsync();

            bool isAdded = workerMgrSvc.TryAddWorker("AAA", "121212", nameof(Colors.CornflowerBlue), Infrastructure.DayOfWeekFlag.Monday, out WorkerModel? newWorker);
            Assert.IsFalse(isAdded);
        }

        [TestMethod]
        public async Task AddNewWorker()
        {
            var logger = CreateLogger<WorkerManageService>();
            var config = GetConfig();

            WorkerManageService workerMgrSvc = new WorkerManageService(logger, config);
            
            // Loaded Ids:1, 2, 3
            await workerMgrSvc.LoadWorkersAsync();

            bool isAdded = workerMgrSvc.TryAddWorker("ABC", "121212", nameof(Colors.CornflowerBlue), Infrastructure.DayOfWeekFlag.Monday, out WorkerModel? newWorker);
            
            Assert.IsTrue(isAdded);
            Assert.IsNotNull(newWorker);
            Assert.IsTrue(newWorker.Id == 4);
        }

        [TestMethod("FillNewWorker (include Remove)")]
        public async Task FillNewWorker()
        {
            var logger = CreateLogger<WorkerManageService>();
            var config = GetConfig();

            WorkerManageService workerMgrSvc = new WorkerManageService(logger, config);
            
            // Loaded Ids:1, 2, 3
            await workerMgrSvc.LoadWorkersAsync();

            bool isRemoved = workerMgrSvc.TryRemoveWorker(2, out WorkerModel? removedWorker);
            Assert.IsTrue(isRemoved);

            bool isAdded = workerMgrSvc.TryAddWorker("ABC", "121212", nameof(Colors.CornflowerBlue), Infrastructure.DayOfWeekFlag.Monday, out WorkerModel? newWorker);
            
            Assert.IsTrue(isAdded);
            Assert.IsNotNull(newWorker);
            Assert.IsTrue(newWorker.Id == 2);
        }
    }
}
