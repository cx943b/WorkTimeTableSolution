using System.Diagnostics;
using System.Text.Json;
using System.Windows.Media;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Converters;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Tests
{
    [TestClass]
    public class JsonConverterTest
    {
        JsonSerializerOptions _jsonOpts;
        WorkerModel[] _workers;

        [TestInitialize]
        public void Initialize()
        {
            _jsonOpts = new JsonSerializerOptions();
            _jsonOpts.Converters.Add(new SolidColorBrushJsonConverter());
            _jsonOpts.Converters.Add(new WorkTimeModelJsonConverter());
            _jsonOpts.WriteIndented = true;

            _workers = new WorkerModel[]
            {
                new WorkerModel(1, "AAA", "121111", Brushes.Crimson),
                new WorkerModel(2, "BBB", "111022", Brushes.CornflowerBlue),
                new WorkerModel(3, "CCC", "220923", Brushes.Gold)
            };

            Random rand = new Random();
            DateTime startDateTime = DateTime.Parse("2024-01-01");

            WorkTimeModel? newWorkTime = null;
            int workHour = 0;

            for(int i = 0; i < 4; ++i)
            {
                foreach(var worker in _workers)
                {
                    workHour = rand.Next(0, 24);
                    newWorkTime = new WorkTimeModel(i)
                    {
                        Year = startDateTime.Year,
                        Month = startDateTime.Month,
                        Day = startDateTime.Day,
                        Hour = startDateTime.Hour,
                        Minute = startDateTime.Minute,

                        WorkTimeSpan = TimeSpan.FromHours(workHour)
                    };

                    startDateTime = startDateTime.AddHours(workHour);
                    worker.AddWorkTime(newWorkTime);
                }

                startDateTime.AddDays(rand.Next(2, 5));
            }
        }

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
                WorkerModel worker = _workers.First();

                string jsonStr = JsonSerializer.Serialize(worker, _jsonOpts);
                WorkerModel? revWorker = JsonSerializer.Deserialize<WorkerModel>(jsonStr, _jsonOpts);

                Assert.IsNotNull(revWorker);
                Assert.AreEqual(worker.Id, revWorker.Id);
                Assert.AreEqual(worker.Name, revWorker.Name);
                Assert.AreEqual(worker.Brush.Color, revWorker.Brush.Color);
                Assert.AreEqual(worker.Brush.Opacity, revWorker.Brush.Opacity);
                Assert.AreEqual(worker.FixedWorkWeeks, revWorker.FixedWorkWeeks);
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
                string jsonStr = JsonSerializer.Serialize(_workers, _jsonOpts);
                IEnumerable<WorkerModel>? revWorkers = JsonSerializer.Deserialize<IEnumerable<WorkerModel>>(jsonStr, _jsonOpts);

                Assert.IsNotNull(revWorkers);
                Assert.AreEqual(_workers.Length, revWorkers.Count());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        [TestMethod]
        public void ConvertAndRevertWorkTimeModel()
        {
            WorkTimeModel workTime = _workers.First().WorkTimes.First();

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
            WorkTimeModel[] worktimes = _workers.First().WorkTimes.ToArray();

            string jsonStr = JsonSerializer.Serialize(worktimes, _jsonOpts);
            IEnumerable<WorkTimeModel>? revWorkTimes = JsonSerializer.Deserialize<IEnumerable<WorkTimeModel>>(jsonStr, _jsonOpts);

            Assert.IsNotNull(revWorkTimes);
            Assert.AreEqual(worktimes.Length, revWorkTimes.Count());
        }
    }
}