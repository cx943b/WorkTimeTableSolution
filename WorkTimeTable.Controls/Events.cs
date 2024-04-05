using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WorkTimeTable.Infrastructure.Interfaces;

namespace WorkTimeTable.Controls
{
    
    public delegate void WorkTimeRemoveRequestEventHandler(object sender, WorkTimeRemoveRequestEventArgs e);

    
    public class WorkTimeRemoveRequestEventArgs : RoutedEventArgs
    {
        public IWorkTime TargetWorkTime { get; init; }

        public WorkTimeRemoveRequestEventArgs(IWorkTime targetWorkTime) : base(WorkTimeItemsControl.WorkTimeRemoveRequestEvent)
        {
            TargetWorkTime = targetWorkTime;
        }
    }
}
