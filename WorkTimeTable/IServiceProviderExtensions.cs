using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure;

namespace WorkTimeTable
{
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Retrieves the view instance associated with the specified view name.
        /// </summary>
        /// <param name="provider">The service provider.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <returns>The view instance.</returns>
        public static object? GetView(this IServiceProvider provider, string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(nameof(viewName), "NullRef: viewName");

            IViewTypeService viewTypeSvc = provider.GetRequiredService<IViewTypeService>();
            Type? viewType = viewTypeSvc.GetViewType(viewName);
            if (viewType is null)
                throw new NullReferenceException($"NotFound: {viewName}");

            return provider.GetService(viewType);
        }

        /// <summary>
        /// Retrieves the view model instance associated with the specified view name.
        /// </summary>
        /// <param name="provider">The service provider.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <returns>The view model instance.</returns>
        public static object? GetViewModel(this IServiceProvider provider, string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(nameof(viewName), "NullRef: viewName");

            IViewTypeService viewTypeSvc = provider.GetRequiredService<IViewTypeService>();
            Type? viewModelType = viewTypeSvc.GetViewModelType(viewName);
            if (viewModelType is null)
                throw new NullReferenceException($"NotFound: {viewName}");

            return provider.GetService(viewModelType);
        }
    }
}