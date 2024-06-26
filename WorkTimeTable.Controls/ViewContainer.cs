﻿using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WorkTimeTable.Infrastructure;

namespace WorkTimeTable.Controls
{
    public class ViewContainer : ContentControl
    {
        public static readonly DependencyProperty ViewNameProperty = DependencyProperty.Register(nameof(ViewName), typeof(string), typeof(ViewContainer), new UIPropertyMetadata(null, onViewNamePropertyChanged));
        public static readonly DependencyProperty DesignTimeContentTemplateProperty = DependencyProperty.Register(nameof(DesignTimeContentTemplate), typeof(DataTemplate), typeof(ViewContainer), new UIPropertyMetadata(null, onDesignTimeContentTemplateProperty));

        public string ViewName
        {
            get => (string)GetValue(ViewNameProperty);
            set => SetValue(ViewNameProperty, value);
        }
        public DataTemplate DesignTimeContentTemplate
        {
            get => (DataTemplate)GetValue(DesignTimeContentTemplateProperty);
            set => SetValue(DesignTimeContentTemplateProperty, value);
        }


        protected virtual void OnViewNameChanged(string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(newValue))
            {
                Content = null;
                return;
            }

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                IViewTypeService viewTypeSvc = Ioc.Default.GetRequiredService<IViewTypeService>();
                Type? viewType = viewTypeSvc.GetViewType(newValue);

                if (viewType is null)
                    throw new TypeLoadException($"NotFoundViewType: {newValue}");

                var view = (FrameworkElement)Ioc.Default.GetRequiredService(viewType);
                Content = view;

                // Set viewModel if type exist
                Type? viewModelType = viewTypeSvc.GetViewModelType(newValue);

                if (viewModelType is not null)
                {
                    var viewModel = Ioc.Default.GetRequiredService(viewModelType);
                    view.DataContext = viewModel;
                }
            }
        }
        protected virtual void OnDesignTimeContentTemplate(DataTemplate oldValue, DataTemplate newValue)
        {
            if (newValue == null)
                return;

            if(System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Content = newValue.LoadContent();
            }
        }

        private static void onViewNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ViewContainer viewContainer)
            {
                viewContainer.OnViewNameChanged((string)e.OldValue, (string)e.NewValue);
            }
        }
        private static void onDesignTimeContentTemplateProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is ViewContainer viewContainer)
            {
                viewContainer.OnDesignTimeContentTemplate((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
            }
        }
    }
}
