using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoWeb.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace InfoWeb.DistributedServices.InputModels
{
    public class CreateAssignmentInputModel
    {
        [Required]
        public Project Project { get; set; }

        [Required]
        public User User { get; set; }
        
        public HourType HourType { get; set; }

        public int Hours { get; set; }
    }
}
