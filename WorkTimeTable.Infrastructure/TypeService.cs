using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable.Infrastructure
{
    public class TypeService : ITypeService, IViewTypeService
    {
        static IEnumerable<Assembly> _targetAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        public static Func<string, string> GetViewModelNameLogic => GetViewModelName;

        public static void ChangeTargetAssembliesStartWith(string? startWithName)
        {
            if(String.IsNullOrEmpty(startWithName))
            {
                _targetAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            else
            {
                _targetAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(ass => !String.IsNullOrEmpty(ass.FullName) && ass.FullName.StartsWith(startWithName, StringComparison.OrdinalIgnoreCase))
                    .ToArray();
            }
        }

        public Type? GetViewType(string viewName)
        {
            if (String.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(viewName, "viewName is null or empty");

            return GetType(viewName);
        }

        public Type? GetViewModelType(string viewName)
        {
            if (String.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(viewName, "viewName is null or empty");

            string definedViewModelName = GetViewModelNameLogic(viewName);
            return GetType(definedViewModelName);
        }

        public Type? GetType(string typeName)
        {
            if (String.IsNullOrEmpty(typeName))
                throw new ArgumentNullException(nameof(typeName));

            Type? viewModelType = null;

            foreach (var assembly in _targetAssemblies)
            {
                viewModelType = assembly.DefinedTypes.FirstOrDefault(t => String.Compare(t.Name, typeName, true) == 0);
                if (viewModelType is not null)
                    return viewModelType;
            }

            return null;
        }

        private static string GetViewModelName(string viewName) => $"{viewName}Model";
    }

    public interface ITypeService
    {
        Type? GetType(string typeName);
    }
    public interface IViewTypeService
    {
        Type? GetViewType(string viewName);
        Type? GetViewModelType(string viewName);
    }
}
