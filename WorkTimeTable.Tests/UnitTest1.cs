using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Media;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Converters;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;


namespace WorkTimeTable.Tests
{
    [TestClass]
    public class JsonConverterTest
    {
        JsonSerializerOptions _jsonOpts;

        [TestInitialize]
        public void Initialize()
        {
            _jsonOpts = new JsonSerializerOptions();
            _jsonOpts.Converters.Add(new SolidColorBrushJsonConverter());
            _jsonOpts.WriteIndented = true;
        }



        [TestMethod]
        public void ConvertTest()
        {
            var workers = new WorkerModel[]
                {
                    new WorkerModel(0, "AAA", Brushes.Crimson,
                        new WorkTimeModel[]
                        {
                            new WorkTimeModel(0)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-01"), TimeOnly.Parse("00:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            },
                            new WorkTimeModel(0)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-02"), TimeOnly.Parse("12:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            }
                        }),
                    new WorkerModel(1, "BBB", Brushes.CornflowerBlue,
                        new WorkTimeModel[]
                        {
                            new WorkTimeModel(1)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-01"), TimeOnly.Parse("12:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            },
                            new WorkTimeModel(1)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-03"), TimeOnly.Parse("00:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            }
                        }),
                    new WorkerModel(2, "CCC", Brushes.Gold,
                        new WorkTimeModel[]
                        {
                            new WorkTimeModel(2)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-02"), TimeOnly.Parse("00:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            },
                            new WorkTimeModel(2)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-03"), TimeOnly.Parse("12:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            }
                        })
                };

            string jsonStr= JsonSerializer.Serialize(workers, _jsonOpts);
            Debug.WriteLine(jsonStr);
        }

        [TestMethod]
        public void ConvertAndRevertSolidColorBrush()
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromRgb(111, 222, 123);
            brush.Opacity = 0.4;

            JsonSerializerOptions _jsonOpts = new JsonSerializerOptions();
            _jsonOpts.Converters.Add(new SolidColorBrushJsonConverter());
            _jsonOpts.WriteIndented = true;

            string jsonStr = JsonSerializer.Serialize(brush, _jsonOpts);

            SolidColorBrush? revBrush = JsonSerializer.Deserialize<SolidColorBrush>(jsonStr, _jsonOpts);
            Assert.IsNotNull(revBrush);
            Assert.AreEqual(brush.Color, revBrush.Color);
            Assert.AreEqual(brush.Opacity, revBrush.Opacity);
        }

        [TestMethod]
        public void ConvertAndRevertWorkerModel()
        {
            WorkerModel worker = new WorkerModel(0, "AAA", Brushes.Crimson,
                        DayOfWeekFlag.Tuesday,
                        [
                            new WorkTimeModel(0)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-01"), TimeOnly.Parse("00:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            },
                            new WorkTimeModel(0)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-02"), TimeOnly.Parse("12:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            }
                        ]);

            JsonSerializerOptions _jsonOpts = new JsonSerializerOptions();
            _jsonOpts.Converters.Add(new SolidColorBrushJsonConverter());
            _jsonOpts.WriteIndented = true;

            string jsonStr = JsonSerializer.Serialize<WorkerModel>(worker, _jsonOpts);

            WorkerModel? revWorker = JsonSerializer.Deserialize<WorkerModel>(jsonStr, _jsonOpts);
            Assert.IsNotNull(revWorker);
            Assert.AreEqual(worker.Id, revWorker.Id);
            Assert.AreEqual(worker.Name, revWorker.Name);
            Assert.AreEqual(worker.Brush.Color, revWorker.Brush.Color);
            Assert.AreEqual(worker.Brush.Opacity, revWorker.Brush.Opacity);
            Assert.AreEqual(worker.FixedWorkWeeks, revWorker.FixedWorkWeeks);
            Assert.AreEqual(worker.WorkTimes.Count, revWorker.WorkTimes.Count);
        }

        [TestMethod]
        public void ConvertAndRevertWorkTimeModel()
        {
            JsonSerializerOptions _jsonOpts = new JsonSerializerOptions();
            _jsonOpts.WriteIndented = true;

            WorkTimeModel workTime = new WorkTimeModel(0)
            {
                StartTime = new DateTime(DateOnly.Parse("2024-01-02"), TimeOnly.Parse("12:00:00")),
                WorkTimeSpan = TimeSpan.FromHours(12)
            };

            string jsonStr = JsonSerializer.Serialize<WorkTimeModel>(workTime, _jsonOpts);
            WorkTimeModel? revWorkTime = JsonSerializer.Deserialize<WorkTimeModel>(jsonStr, _jsonOpts);

            Assert.IsNotNull(revWorkTime);
            Assert.AreEqual(workTime.StartTime, revWorkTime.StartTime);
            Assert.AreEqual(workTime.WorkTimeSpan, revWorkTime.WorkTimeSpan);
            Assert.AreEqual(workTime.WorkerId, revWorkTime.WorkerId);

            // ---------------------------------

            WorkTimeModel[] worktimes =
                        [
                            new WorkTimeModel(0)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-01"), TimeOnly.Parse("00:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            },
                            new WorkTimeModel(0)
                            {
                                StartTime = new DateTime(DateOnly.Parse("2024-01-02"), TimeOnly.Parse("12:00:00")),
                                WorkTimeSpan = TimeSpan.FromHours(12)
                            }
                        ];

            jsonStr = JsonSerializer.Serialize<WorkTimeModel[]>(worktimes, _jsonOpts);
            WorkTimeModel[]? revWorkTimes = JsonSerializer.Deserialize<WorkTimeModel[]>(jsonStr, _jsonOpts);

            Assert.IsNotNull(revWorkTimes);
            Assert.AreEqual(worktimes.Length, revWorkTimes.Length);
        }
    }
}
