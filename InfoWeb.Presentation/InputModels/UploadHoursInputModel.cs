using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.Presentation.InputModels
{
    public class UploadHoursInputModel
    {
        [Required]
        public Project Project { get; set; }

        [Required]
        public ProjectsHoursTypes ProjectHourType { get; set; }

        [Required]
        public string StartDate { get; set; }

        [Required]
        public string EndDate { get; set; }

        public string Description { get; set; }
    }
}
