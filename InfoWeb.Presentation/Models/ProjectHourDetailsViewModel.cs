using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.Presentation.Models
{
    public class ProjectHourDetailsViewModel
    {
        public string ProjectName { get; set; }
        public string HourTypeName { get; set; }
        public int TotalHours { get; set; }
        public int TotalHoursAssigned { get; set; }
    }
}
