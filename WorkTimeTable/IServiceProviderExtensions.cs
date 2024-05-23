using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable
{
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Gets the service for the specified view model.
        /// </summary>
        /// <param name="provider">The service provider.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <returns>The service object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="viewName"/> is null or empty.</exception>
        public static object? GetServiceForViewModel(this IServiceProvider provider, string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException(nameof(viewName), "NullRef: viewName");
            }

            Type viewType = Type.GetType(viewName) ?? throw new NullReferenceException($"NullRef: {nameof(viewType)}");

            string viewNameSpace = viewType.Namespace ?? throw new NullReferenceException($"NullRef: {nameof(viewNameSpace)}");
            string viewModelNameSpace = viewNameSpace.Replace("Views", "ViewModels");

            string viewModelName = $"{viewName}Model";
            Type viewModelType = Type.GetType($"{viewModelNameSpace}.{viewModelName}") ?? throw new NullReferenceException($"NotFound: {viewModelName} type");

            return provider.GetService(viewModelType);
        }
    }
}