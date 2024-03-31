using System.Windows.Media;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.Infrastructure.Interfaces
{
    public interface  IWorker
    {
        int Id { get; set; }
        string Name { get; set; }
        SolidColorBrush Brush { get; set; }

        List<WorkTimeModel> WorkTimes { get; set; }
    }
}
