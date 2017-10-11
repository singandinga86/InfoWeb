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
    [Route("api/user/{userId}/Assignments")]
    public class AssignmentsController: Controller
    {
        private readonly IAssignmentRepository assigmentRepository;       
        public AssignmentsController(IAssignmentRepository assigmentRepository)
        {
            this.assigmentRepository = assigmentRepository;           
        }

        [HttpGet("Projects")]
        public IEnumerable<Assignment> GetProjects(int userId)
        {
            IEnumerable<Assignment> result = assigmentRepository.GetAssignments(userId);
            return result;
        }

        //[HttpGet("Hours")]
        //public IEnumerable<Assignment> GetHours(int userId)
        //{
        //    IEnumerable<Assignment> result = assigmentRepository.GetAssignments(userId, AssignmentType.AssignmentTypeHours);
        //    return result;
        //}

     

    }
}
