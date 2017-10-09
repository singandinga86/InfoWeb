using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InfoWeb.Domain.Entities
{
    public class AssignmentType
    {
        public static string AssignmentTypeHours { get => "Hours"; }
        public static string AssignmentTypeProject { get => "Projects"; }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
