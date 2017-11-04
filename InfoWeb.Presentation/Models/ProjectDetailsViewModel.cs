using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Presentation.Models
{
    public class ProjectDetailsViewModel
    {
       public ProjectDetailsViewModel()
        {
            this.Details = new List<ProjectDetailsUserAssigmentViewModel>();
        }

        public User User { get; set; }
        public Project Project { get; set; }
        public IEnumerable<ProjectDetailsUserAssigmentViewModel> Details { get; set; }

        public IEnumerable<Assignment> Assignments { get; set; }
    }
}
