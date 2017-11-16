using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.Presentation.InputModels
{
    public class ProjectInputModel
    {
        [Required]
        public ProjectType Type { get; set; }
        [Required]
        public Client Client { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
