using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InfoWeb.Domain.Entities
{
    public class WorkedHour
    {
        public int Id { get; set; }

        [Required]
        public DateTime DateBegin { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }

        [Required]
        public int Hours { get; set; }

        [Required]
        public ProjectsHoursTypes ProjectHourType { get; set; }

        public int ProjectHourTypeId { get; set; }

        [Required]
        public User User { get; set; }
        
        public int UserId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
