using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

    public class TypeFinder
    {
        public static Type? FromName(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type is not null)
            {
                return type;
            }

            var appDomain = AppDomain.CurrentDomain;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.FullName.StartsWith(appDomain.FriendlyName, StringComparison.OrdinalIgnoreCase)))
            {
                type = asm.GetType(typeName);
                if (type is not null)
                {
                    return type;
                }
            }

            return null;
        }
    }
}
