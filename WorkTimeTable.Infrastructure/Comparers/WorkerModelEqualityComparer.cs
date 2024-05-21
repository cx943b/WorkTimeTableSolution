using System.Diagnostics.CodeAnalysis;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Comparers
{
    public class WorkerModelEqualityComparer : IEqualityComparer<WorkerModel>
    {
        public bool Equals(WorkerModel? x, WorkerModel? y)
        {
            if (x == null || y == null)
                return false;

            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] WorkerModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
