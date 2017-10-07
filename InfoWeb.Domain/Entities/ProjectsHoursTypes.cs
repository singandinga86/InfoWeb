using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InfoWeb.Domain.Entities
{
    public class ProjectsHoursTypes
    {
        public int Id { get; set; }

        public HourType HourType { get; set; }

        [Range(1, int.MaxValue)]
        public int HourTypeId { get; set; }

        public Project Project { get; set; }

        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }

        [Range(1, int.MaxValue)]
        public int Hours { get; set; }
    }
}
