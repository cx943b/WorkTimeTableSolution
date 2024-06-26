using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Messages
{
    public enum WorkerListChangedStatus { Added, Removed }

    public class WorkerListChangedMessageArgs
    {
        public WorkerListChangedStatus Status { get; init; }
        public IEnumerable<IWorker> Workers { get; init; } = Enumerable.Empty<WorkerModel>();

        public WorkerListChangedMessageArgs(WorkerListChangedStatus status, IEnumerable<IWorker> workers)
        {
            if (workers is null) throw new ArgumentNullException(nameof(workers));

            Status = status;
            Workers = workers;
        }
    }

    public class WorkerListChangedMessage : ValueChangedMessage<WorkerListChangedMessageArgs>
    {
        public WorkerListChangedMessage(WorkerListChangedMessageArgs args) : base(args)
        {

        }
    }
}
