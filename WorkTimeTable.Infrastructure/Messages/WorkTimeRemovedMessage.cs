using CommunityToolkit.Mvvm.Messaging.Messages;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Messages
{
    public class WorkTimeRemovedMessage : ValueChangedMessage<WorkTimeRemovedMessageArgs>
    {
        public WorkTimeRemovedMessage(WorkTimeRemovedMessageArgs args) : base(args)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));
        }
    }

    public class WorkTimeRemovedMessageArgs
    {
        public WorkTimeModel OldWorkTime { get; init; }

        public WorkTimeRemovedMessageArgs(WorkTimeModel oldWorkTime)
        {
            OldWorkTime = oldWorkTime ?? throw new ArgumentNullException(nameof(oldWorkTime));
        }
    }
}
