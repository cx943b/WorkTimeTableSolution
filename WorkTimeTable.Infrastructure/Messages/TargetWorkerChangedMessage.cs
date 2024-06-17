using CommunityToolkit.Mvvm.Messaging.Messages;
using WorkTimeTable.Infrastructure.Interfaces;

namespace WorkTimeTable.Infrastructure.Messages
{
    public class TargetWorkerInfoChangedMessage : ValueChangedMessage<IWorker>
    {
        public TargetWorkerInfoChangedMessage(IWorker worker) : base(worker)
        {
            if(worker == null)
            {
                throw new System.ArgumentNullException(nameof(worker));
            }
        }
    }
    public class TargetWorkerChangedMessage : ValueChangedMessage<IWorker?>
    {
        public TargetWorkerChangedMessage(IWorker? worker) : base(worker)
        {

        }
    }
}
