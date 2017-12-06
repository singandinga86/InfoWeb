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
        public HourType HourType { get; set; }

        [Required]
        public string DateStart { get; set; }

        [Required]
        public string DateEnd { get; set; }

        public string Description { get; set; }
    }
}
