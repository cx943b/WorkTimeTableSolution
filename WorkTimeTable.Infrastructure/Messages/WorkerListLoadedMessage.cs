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
    public class WorkerListLoadedMessage : ValueChangedMessage<WorkerListLoadedMessageArgs>
    {
        public WorkerListLoadedMessage(WorkerListLoadedMessageArgs args) : base(args)
        {

        }
    }

    public class WorkerListLoadedMessageArgs
    {
        public IEnumerable<IWorker> Workers { get; init; }

        public WorkerListLoadedMessageArgs(IEnumerable<IWorker> workers)
        {
            if (workers is null) throw new ArgumentNullException(nameof(workers));

            Workers = workers;
        }
    }
}
