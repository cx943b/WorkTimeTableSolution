using CommunityToolkit.Mvvm.Messaging.Messages;
using WorkTimeTable.Infrastructure.Interfaces;

namespace WorkTimeTable.Infrastructure.Messages
{
    public class TargetWorkerChangedMessage : ValueChangedMessage<IWorker?>
    {
        public TargetWorkerChangedMessage(IWorker? worker) : base(worker)
        {

        }
    }
}
