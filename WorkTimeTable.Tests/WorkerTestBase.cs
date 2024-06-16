using System.Text.Json;
using System.Windows.Media;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Converters;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Tests
{
    public class WorkerTestBase
    {
        readonly WorkerModel[] _workers;
        readonly JsonSerializerOptions _options;

        protected WorkerModel[] Workers => _workers;
        protected JsonSerializerOptions Options => _options;

        public WorkerTestBase()
        {
            _options = new JsonSerializerOptions();
            _options.WriteIndented = true;
            _options.Converters.Add(new WorkTimeModelJsonConverter());
            _options.Converters.Add(new WorkerModelJsonConverter());

            _workers = new WorkerModel[]
            {
                new WorkerModel { Id = 1, Name = "AAA", BirthDate = "121111", ColorName = nameof(Colors.Crimson) },
                new FixedWorkerModel { Id = 2, Name = "BBB", BirthDate = "111022", ColorName = nameof(Colors.CornflowerBlue), FixedWorkWeeks = DayOfWeekFlag.Saturday | DayOfWeekFlag.Friday },
                new WorkerModel { Id = 3, Name = "CCC", BirthDate = "220923", ColorName = nameof(Colors.Gold)}
            };

            Random rand = new Random();
            DateTime startDateTime = DateTime.Parse("2024-01-01");

            WorkTimeModel? newWorkTime = null;
            int workHour = 0;

            for (int i = 0; i < 10; ++i)
            {
                foreach (var worker in _workers)
                {
                    workHour = rand.Next(0, 24);
                    newWorkTime = new WorkTimeModel()
                    {
                        Year = startDateTime.Year,
                        Month = rand.Next(1, 3),
                        Day = startDateTime.Day,
                        Hour = startDateTime.Hour,
                        Minute = startDateTime.Minute,

                        WorkerId = worker.Id,
                        WorkTimeSpan = TimeSpan.FromHours(workHour)
                    };

                    startDateTime = startDateTime.AddHours(workHour);
                    worker.AddWorkTime(newWorkTime);
                }

                startDateTime.AddDays(rand.Next(2, 5));
            }
        }
    }
}