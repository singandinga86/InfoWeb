using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoWeb.Domain.Entities;

namespace InfoWeb.DistributedServices.InputModels
{
    public class CreateAssignmentInputModel
    {
        public Project Project { get; set; }
        public User User { get; set; }
        
        public HourType HourType { get; set; }
        public int Hours { get; set; }
    }
}
