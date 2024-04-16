using SosoThemeLibrary.Extensions;
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
    public class WorkTimeItemsControl : ItemsControl
    {
        static readonly int[] _Months = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        static readonly int[] _Days = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        static readonly int[] _Hours = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        static readonly int[] _Minutes = new int[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };

        public IEnumerable<int> Months => _Months;
        public IEnumerable<int> Days => _Days;
        public IEnumerable<int> Hours => _Hours;
        public IEnumerable<int> Minutes => _Minutes;

        public event WorkTimeRemoveRequestEventHandler WorkTimeRemoveRequest
        {
            add => AddHandler(WorkTimeItemsControl.WorkTimeRemoveRequestEvent, value);
            remove => RemoveHandler(WorkTimeItemsControl.WorkTimeRemoveRequestEvent, value);
        }
        public event ScrollChangedEventHandler ScrollChanged
        {
            add => AddHandler(WorkTimeItemsControl.ScrollChangedEvent, value);
            remove => RemoveHandler(WorkTimeItemsControl.ScrollChangedEvent, value);
        }

        public static readonly RoutedEvent ScrollChangedEvent = ScrollViewer.ScrollChangedEvent.AddOwner(typeof(WorkTimeItemsControl));
        public static readonly RoutedEvent WorkTimeRemoveRequestEvent = EventManager.RegisterRoutedEvent("WorkTimeRemoveRequest", RoutingStrategy.Bubble, typeof(WorkTimeRemoveRequestEventHandler), typeof(WorkTimeItemsControl));


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //ScrollViewer? scrViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            //if(scrViewer != null)
            //{
            //    scrViewer.ScrollChanged += onScrollChanged;
            //}
        }


        protected override DependencyObject GetContainerForItemOverride()
        {
            return new WorkTimeItem();
        }

        private static void onScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrViewer = sender as ScrollViewer;
        }
    }
}