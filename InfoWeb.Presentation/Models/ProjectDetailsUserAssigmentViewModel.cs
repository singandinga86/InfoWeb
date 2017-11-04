using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.Presentation.Models
{
    public class ProjectDetailsUserAssigmentViewModel
    {
        public ProjectDetailsUserAssigmentViewModel()
        {
            InnerAssignments = new List<ProjectDetailsUserAssigmentViewModel>();
        }
        public User User { get; set; }
        public IEnumerable<Assignment> Assignments { get; set; }

        public IEnumerable<ProjectDetailsUserAssigmentViewModel> InnerAssignments { get; set; }
    }
}
