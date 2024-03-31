using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkTimeTable.Infrastructure.Interfaces;

namespace WorkTimeTable.Controls
{
    public class WorkTimeItem : Control
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button? btnRemove = GetTemplateChild("btnRemove") as Button;
            if (btnRemove != null)
                btnRemove.Click += onBtnRemoveClick;
        }

        private static void onBtnRemoveClick(object sender, RoutedEventArgs e)
        {
            WorkTimeItem? item = sender as WorkTimeItem;
            if (item != null && item.DataContext is IWorkTime workTime)
            {
                item.RaiseEvent(new WorkTimeRemoveRequestEventArgs(workTime));
            }
        }
    }
}
