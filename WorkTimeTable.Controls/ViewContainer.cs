using CommunityToolkit.Mvvm.DependencyInjection;
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

        public string ViewName
        {
            get => (string)GetValue(ViewNameProperty);
            set => SetValue(ViewNameProperty, value);
        }


        protected virtual void OnViewNameChanged(string oldValue, string newValue)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                Content = new TextBlock()
                {
                    Text = $"{newValue}{nameof(ViewContainer)}",
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
            }
            else
            {
                if (string.IsNullOrEmpty(newValue))
                {
                    Content = null;
                }
                else
                {
                    IViewTypeService viewTypeSvc = Ioc.Default.GetRequiredService<IViewTypeService>();

                    Type? viewType = viewTypeSvc.GetViewType(newValue);
                    if (viewType is null)
                        throw new TypeLoadException($"NotFoundViewType: {newValue}");

                    var view = (FrameworkElement)Ioc.Default.GetRequiredService(viewType);
                    Content = view;

                    Type? viewModelType = viewTypeSvc.GetViewModelType(newValue);

                    if (viewModelType is not null)
                    {
                        var viewModel = Ioc.Default.GetService(viewModelType);
                        if (viewModel is null)
                            throw new TypeLoadException($"Exist ViewModelType but registered");

                        view.DataContext = viewModel;
                    }
                }
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
