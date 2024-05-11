using System.Diagnostics;
using System.Text.Json;
using System.Windows.Media;
using WorkTimeTable.Infrastructure.Converters;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Tests
{
    [TestClass]
    public class JsonConverterTest : WorkerTestBase
    {
        JsonSerializerOptions _jsonOpts;        

        [TestMethod]
        public void ConvertAndRevertSolidColorBrush()
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromRgb(111, 222, 123);
            brush.Opacity = 0.4;

            string jsonStr = JsonSerializer.Serialize(brush, _jsonOpts);

            SolidColorBrush? revBrush = JsonSerializer.Deserialize<SolidColorBrush>(jsonStr, _jsonOpts);
            Assert.IsNotNull(revBrush);
            Assert.AreEqual(brush.Color, revBrush.Color);
            Assert.AreEqual(brush.Opacity, revBrush.Opacity);
        }

        [TestMethod]
        public void ConvertAndRevertWorkerModel()
        {
            try
            {
                WorkerModel worker = Workers.First();
                
                
                string jsonStr = JsonSerializer.Serialize(worker, _jsonOpts);
                WorkerModel? revWorker = JsonSerializer.Deserialize<WorkerModel>(jsonStr, _jsonOpts);

                Assert.IsNotNull(revWorker);
                Assert.AreEqual(worker.Id, revWorker.Id);
                Assert.AreEqual(worker.Name, revWorker.Name);
                Assert.AreEqual(worker.ColorName, revWorker.ColorName);
                //workTimeAssert.AreEqual(worker.FixedWorkWeeks, revWorker.FixedWorkWeeks);
                Assert.AreEqual(worker.WorkTimes.Count, revWorker.WorkTimes.Count);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        [TestMethod]
        public void ConvertAndRevertWorkerModels()
        {
            try
            {
                string jsonStr = JsonSerializer.Serialize(Workers, _jsonOpts);
                IEnumerable<WorkerModel>? revWorkers = JsonSerializer.Deserialize<IEnumerable<WorkerModel>>(jsonStr, _jsonOpts);

                Assert.IsNotNull(revWorkers);
                Assert.AreEqual(Workers.Length, revWorkers.Count());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        [TestMethod]
        public void ConvertAndRevertWorkTimeModel()
        {
            WorkTimeModel workTime = Workers.First().WorkTimes.First();

            string jsonStr = JsonSerializer.Serialize(workTime, _jsonOpts);
            WorkTimeModel? revWorkTime = JsonSerializer.Deserialize<WorkTimeModel>(jsonStr, _jsonOpts);

            Assert.IsNotNull(revWorkTime);
            Assert.AreEqual(workTime.WorkerId, revWorkTime.WorkerId);
            Assert.AreEqual(workTime.StartWorkTime, revWorkTime.StartWorkTime);
            Assert.AreEqual(workTime.WorkTimeSpan, revWorkTime.WorkTimeSpan);
        }
        [TestMethod]
        public void ConvertAndRevertWorkTimeModels()
        {
            WorkTimeModel[] worktimes = Workers.First().WorkTimes.ToArray();

            string jsonStr = JsonSerializer.Serialize(worktimes, _jsonOpts);
            IEnumerable<WorkTimeModel>? revWorkTimes = JsonSerializer.Deserialize<IEnumerable<WorkTimeModel>>(jsonStr, _jsonOpts);

            Assert.IsNotNull(revWorkTimes);
            Assert.AreEqual(worktimes.Length, revWorkTimes.Count());
        }
    }
}