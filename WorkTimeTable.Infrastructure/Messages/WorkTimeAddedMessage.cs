using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Messages
{
    public class WorkTimeAddedMessage : ValueChangedMessage<WorkTimeAddedMessageArgs>
    {
        public WorkTimeAddedMessage(WorkTimeAddedMessageArgs args) : base(args)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));
        }
    }

    public class WorkTimeAddedMessageArgs
    {
        public WorkTimeModel NewWorkTime { get; init; }

        public WorkTimeAddedMessageArgs(WorkTimeModel newWorkTime)
        {
            NewWorkTime = newWorkTime ?? throw new ArgumentNullException(nameof(newWorkTime));
        }
    }

    
}
