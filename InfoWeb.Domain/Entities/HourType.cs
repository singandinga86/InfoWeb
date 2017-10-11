using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InfoWeb.Domain.Entities
{
    public class HourType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ProjectsHoursTypes> ProjectsHoursTypes { get; set; }
    }
}
