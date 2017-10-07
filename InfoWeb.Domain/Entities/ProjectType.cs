using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InfoWeb.Domain.Entities
{
    public class ProjectType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
