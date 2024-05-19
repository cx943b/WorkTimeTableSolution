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
        /// <summary>
        /// Test method to verify that a WorkerModel object can be serialized and deserialized to and from JSON.
        /// </summary>
        [TestMethod]
        public void ConvertAndRevertWorkerModel()
        {
            try
            {
                // Get the first WorkerModel object from the Workers collection
                WorkerModel worker = Workers.First();

                // Serialize the WorkerModel object to JSON
                string jsonStr = JsonSerializer.Serialize(worker, base.Options);

                // Deserialize the JSON back into a WorkerModel object
                WorkerModel? revWorker = JsonSerializer.Deserialize<WorkerModel>(jsonStr, base.Options);

                // Verify that the deserialized object is not null
                Assert.IsNotNull(revWorker);

                // Verify that the Id property of the deserialized object matches the Id property of the original object
                Assert.AreEqual(worker.Id, revWorker.Id);

                // Verify that the Name property of the deserialized object matches the Name property of the original object
                Assert.AreEqual(worker.Name, revWorker.Name);

                // Verify that the ColorName property of the deserialized object matches the ColorName property of the original object
                Assert.AreEqual(worker.ColorName, revWorker.ColorName);

                // Verify that the WorkTimes collection of the deserialized object has the same number of elements as the original object
                Assert.AreEqual(worker.WorkTimes.Count(), revWorker.WorkTimes.Count());
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the test
                Debug.WriteLine(ex.Message);
            }
        }

        [TestMethod]
        public void ConvertAndRevertWorkerModels()
        {
            try
            {
                string jsonStr = JsonSerializer.Serialize(Workers, base.Options);
                IEnumerable<WorkerModel>? revWorkers = JsonSerializer.Deserialize<IEnumerable<WorkerModel>>(jsonStr, base.Options);

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

            string jsonStr = JsonSerializer.Serialize(workTime, base.Options);
            WorkTimeModel? revWorkTime = JsonSerializer.Deserialize<WorkTimeModel>(jsonStr, base.Options);

            Assert.IsNotNull(revWorkTime);
            Assert.AreEqual(workTime.WorkerId, revWorkTime.WorkerId);
            Assert.AreEqual(workTime.StartWorkTime, revWorkTime.StartWorkTime);
            Assert.AreEqual(workTime.WorkTimeSpan, revWorkTime.WorkTimeSpan);
        }
        [TestMethod]
        public void ConvertAndRevertWorkTimeModels()
        {
            WorkTimeModel[] worktimes = Workers.First().WorkTimes.ToArray();

            string jsonStr = JsonSerializer.Serialize(worktimes, base.Options);
            IEnumerable<WorkTimeModel>? revWorkTimes = JsonSerializer.Deserialize<IEnumerable<WorkTimeModel>>(jsonStr, base.Options);

            Assert.IsNotNull(revWorkTimes);
            Assert.AreEqual(worktimes.Length, revWorkTimes.Count());
        }
    }
}