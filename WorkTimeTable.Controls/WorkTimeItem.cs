using SosoThemeLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkTimeTable.Infrastructure.Interfaces;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Controls
{
    public class WorkTimeItem : ContentControl
    {
        public bool IsTenPoint
        {
            get => (bool)GetValue(IsTenPointProperty);
            private set => SetValue(IsTenPointPropertyKey, value);
        }
        public bool IsNewAdded
        {
            get { return (bool)GetValue(IsNewAddedProperty); }
            set { SetValue(IsNewAddedProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button? btnRemove = GetTemplateChild("btnRemove") as Button;
            if (btnRemove != null)
                btnRemove.Click += onBtnRemoveClick;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (String.Compare(e.Property.Name, "AlternationIndex", true) == 0)
                onAlternationIndexChanged(this, e);
        }

        private void onAlternationIndexChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            int index = (int)e.NewValue;
            IsTenPoint = index % 10 == 0;
        }

        private static void onBtnRemoveClick(object sender, RoutedEventArgs e)
        {
            var depObj = sender as DependencyObject;
            if (depObj == null)
                return;

            var item = depObj.FindVisualParent<WorkTimeItem>();
            if (item != null && item.DataContext is IWorkTime workTime)
                item.RaiseEvent(new WorkTimeRemoveRequestEventArgs(workTime));
        }

        private static readonly DependencyPropertyKey IsTenPointPropertyKey = DependencyProperty.RegisterReadOnly("IsTenPoint", typeof(bool), typeof(WorkTimeItem), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsTenPointProperty = IsTenPointPropertyKey.DependencyProperty;
        public static readonly DependencyProperty IsNewAddedProperty = DependencyProperty.Register("IsNewAdded", typeof(bool), typeof(WorkTimeItem), new UIPropertyMetadata(false));
    }
}
