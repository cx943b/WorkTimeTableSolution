using System.Windows.Media;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface  IWorker
    {
        int Id { get; set; }
        string Name { get; set; }
        string ColorName { get; set; }

        List<WorkTimeModel> WorkTimes { get; set; }
    }
}
