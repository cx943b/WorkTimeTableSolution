using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable
{
    public static class IServiceProviderExtensions
    {
        public static object? GetServiceForViewModel(this IServiceProvider provider, string viewName)
        {
            string viewModelName = $"{viewName}Model";
            Type? viewModelType = Type.GetType($"WorkTimeTable.ViewModels.{viewModelName}");

            if (viewModelType == null)
                return null;

            return provider.GetService(viewModelType);
        }
    }
}
