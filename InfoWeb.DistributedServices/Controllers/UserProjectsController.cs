using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DistributedServices.Controllers
{
    [Route("api/user/{userId}/Projects")]
    public class UserProjectsController: Controller
    {
        private readonly IAssignmentRepository assigmentRepository;
        public UserProjectsController(IAssignmentRepository assigmentRepository)
        {
            this.assigmentRepository = assigmentRepository;
        }

        [HttpGet()]
        public IEnumerable<Assignment> List(int userId)
        {
            IEnumerable<Assignment> result = assigmentRepository.Assignments.Include(a => a.Project)
                                        .Where(a => a.AssigneeId == userId && a.AssignmentType.Name == AssignmentType.AssignmentTypeProject)
                                        .Include(a => a.Project.Client)
                                        .Include(a => a.Project.Assignments)
                                        .ToList();
            return result;
        }
    }
}
