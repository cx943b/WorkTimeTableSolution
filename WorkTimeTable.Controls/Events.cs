using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Interfaces;

namespace WorkTimeTable.Controls
{

    public delegate void WorkTimeRemoveRequestEventHandler(object sender, WorkTimeRemoveRequestEventArgs e);

    
    public class WorkTimeRemoveRequestEventArgs : RoutedEventArgs
    {
        public IWorkTime TargetWorkTime { get; init; }

        public WorkTimeRemoveRequestEventArgs(IWorkTime targetWorkTime) : base(WorkTimeItemsControl.WorkTimeRemoveRequestEvent)
        {
            if (targetWorkTime == null)
                throw new ArgumentNullException(nameof(targetWorkTime));

            TargetWorkTime = targetWorkTime;
        }
    }


    public class WorkTimeFilterChangedMessage : ValueChangedMessage<WorkTimeFilter>
    {
        public WorkTimeFilterChangedMessage(WorkTimeFilter filter) :base(filter)
        {
            if(filter == null)
                throw new ArgumentNullException(nameof(filter));
        }
    }
}
