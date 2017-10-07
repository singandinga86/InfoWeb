using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InfoWeb.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Client Client { get; set; }

        [Range(1, int.MaxValue)]
        public int ClientId { get; set; }

        public ProjectType Type { get; set; }

        [Range(1, int.MaxValue)]
        public int TypeId { get; set; }

        public virtual ICollection<ProjectsHoursTypes> ProjectsHoursTypes { get; set; }
    }
}
