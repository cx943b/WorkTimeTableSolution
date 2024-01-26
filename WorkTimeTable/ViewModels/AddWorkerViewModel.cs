using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeTable.Infrastructure;
using WorkTimeTable.Infrastructure.Models;

namespace WorkTimeTable.ViewModels
{
    internal partial class AddWorkerViewModel : ObservableValidator
    {
        [ObservableProperty]
        [Required]
        string _Name;

        [ObservableProperty]
        DayOfWeekFlag _FixedWorkWeeks;

        public AddWorkerViewModel(ILogger<AddWorkerViewModel> logger)
        {
            
        }
    }
}
