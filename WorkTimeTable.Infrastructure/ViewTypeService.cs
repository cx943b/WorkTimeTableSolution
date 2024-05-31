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


        public Type? GetViewTypeFromName(string viewName)
        {
            if (String.IsNullOrEmpty(viewName))
                return null;

            foreach (var targetNamespace in _lstTargetNamespace)
            {
                var viewType = Type.GetType($"{targetNamespace}.{viewName}");
                if (viewType != null)
                    return viewType;
            }

            return null;
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
    }

    public interface IViewTypeService
    {
        IEnumerable<string> TargetNamespaces { get; }
        Type? GetViewTypeFromName(string viewName);
        bool AddTargetNamespace(string targetNamespace);
        bool RemoveTargetNamespace(string targetNamespace);
    }
}
