using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure
{
    public static class IWorkerExtensions
    {
        public static void AddWorkTime(this IWorker worker, WorkTimeModel workTime)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));
            if (workTime == null) throw new ArgumentNullException(nameof(workTime));

            if(worker is WorkerModel workerModel)
                workerModel.WorkTimes.Add(workTime);
        }
        public static void AddWorkTimes(this IWorker worker, IEnumerable<WorkTimeModel> workTimes)
        {
            if (worker == null) throw new ArgumentNullException(nameof(worker));
            if (workTimes == null) throw new ArgumentNullException(nameof(workTimes));

            //if (!workTimes.Any())
            //{
            //    ILoggerFactory logFac = Ioc.Default.GetRequiredService<ILoggerFactory>();
            //    logFac.CreateLogger(nameof(IWorkerExtensions))
            //}

                if (worker is WorkerModel workerModel)
            {
                foreach (var workTime in workTimes)
                    workerModel.WorkTimes.Add(workTime);
            }
        }

        public static bool RemoveWorkTime(this IWorker worker, WorkTimeModel workTime)
        {
            if (worker is WorkerModel workerModel)
                return workerModel.WorkTimes.Remove(workTime);

            return false;
        }
        public static bool RemoveWorkTimes(this IWorker worker, IEnumerable<WorkTimeModel> workTimes)
        {
            bool isAllRemoved = true;

            if (worker is WorkerModel workerModel)
            {
                bool isRemoved = false;
                foreach (var workTime in workTimes)
                {
                    isRemoved = workerModel.WorkTimes.Remove(workTime);
                    if (!isRemoved)
                        isAllRemoved = false;
                }
            }

            return isAllRemoved;
        }
    }
}
