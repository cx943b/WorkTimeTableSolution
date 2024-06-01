using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WorkTimeTable.Infrastructure;

namespace WorkTimeTable.Controls
{
    public class ViewContainer : ContentControl
    {
        public static readonly DependencyProperty ViewNameProperty = DependencyProperty.Register(
            nameof(ViewName),
            typeof(string),
            typeof(ViewContainer),
            new UIPropertyMetadata(null, onViewNamePropertyChanged));

        public string ViewName
        {
           get => (string)GetValue(ViewNameProperty);
            set => SetValue(ViewNameProperty, value);
        }

        static ViewContainer()
        {
            var viewTypeSvc = Ioc.Default.GetRequiredService<IViewTypeService>();
            if (viewTypeSvc is null)
                throw new InvalidOperationException("IViewTypeService is not registered");
        }

        public ViewContainer()
        {
            

        }

        protected virtual void OnViewNameChanged(string oldValue, string newValue)
        {
            if(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            if (string.IsNullOrEmpty(newValue))
            {
                Content = null;
            }
            else
            {
                Type? viewType = TypeFinder.FromName(newValue);
                if (viewType is null)
                    throw new TypeLoadException($"NotFoundViewType: {newValue}");

                var view = Ioc.Default.GetRequiredService(viewType);
                Content = view;
            }
        }

        private static void onViewNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ViewContainer viewContainer)
            {
                viewContainer.OnViewNameChanged((string)e.OldValue, (string)e.NewValue);
            }
        }
    }
}
