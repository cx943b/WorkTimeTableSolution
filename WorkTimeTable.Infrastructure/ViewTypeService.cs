using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeTable.Infrastructure
{
    public class ViewTypeService : IViewTypeService
    {
        readonly List<string> _lstTargetNamespace = new List<string>();

        public IEnumerable<string> TargetNamespaces => _lstTargetNamespace;

        public Type? GetViewType(string viewName)
        {
            if (String.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(viewName, "viewName is null or empty");
            if (!_lstTargetNamespace.Any())
                throw new InvalidOperationException("Empty targetNamespaces");

            return _lstTargetNamespace
                .Select(ns => Type.GetType($"{ns}.{viewName}"))
                .FirstOrDefault(viewType => viewType is not null);
        }

        public Type? GetViewModelType(string viewName)
        {
            if (String.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(viewName, "viewName is null or empty");
            if (!_lstTargetNamespace.Any())
                throw new InvalidOperationException("Empty targetNamespaces");

            return _lstTargetNamespace
                .Select(ns => Type.GetType(CreateViewModelFullNameLogic(ns, viewName)))
                .FirstOrDefault(viewType => viewType is not null);
        }

        public bool AddTargetNamespace(string targetNamespace)
        {
            if (String.IsNullOrEmpty(targetNamespace))
                return false;

            if (_lstTargetNamespace.Contains(targetNamespace))
                return false;

            _lstTargetNamespace.Add(targetNamespace);
            return true;
        }

        public bool RemoveTargetNamespace(string targetNamespace)
        {
            if (String.IsNullOrEmpty(targetNamespace))
                return false;

            return _lstTargetNamespace.Remove(targetNamespace);
        }

        protected virtual string CreateViewModelFullNameLogic(string fullNamespace, string viewName)
        {
            if (String.IsNullOrEmpty(fullNamespace))
                throw new ArgumentNullException(fullNamespace, "fullNamespace is null or empty");
            if (String.IsNullOrEmpty(viewName))
                throw new ArgumentNullException(viewName, "viewName is null or empty");

            return $"{fullNamespace.Replace("Views", "ViewModels")}.{viewName}Model";
        }
    }

    public interface IViewTypeService
    {
        IEnumerable<string> TargetNamespaces { get; }
        Type? GetViewType(string viewName);
        Type? GetViewModelType(string viewName);
        bool AddTargetNamespace(string targetNamespace);
        bool RemoveTargetNamespace(string targetNamespace);
    }
}
