using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using InfoWeb.DistributedServices.InputModels;

namespace InfoWeb.DistributedServices.Controllers
{
    [Route("api/user/{userId}/Assignments")]
    public class AssignmentsController: Controller
    {
        private readonly IAssignmentRepository assigmentRepository;
        private readonly IAssignmentTypeRepository assigmentTypeRepository;
        private readonly IUserRepository userRepository;
        public AssignmentsController(IAssignmentRepository assigmentRepository,
                                     IAssignmentTypeRepository assigmentTypeRepository,
                                     IUserRepository userRepository)
        {
            this.assigmentRepository = assigmentRepository;
            this.assigmentTypeRepository = assigmentTypeRepository;
            this.userRepository = userRepository;
        }

        [HttpGet("Projects")]
        public IEnumerable<Assignment> GetProjects(int userId)
        {
            IEnumerable<Assignment> result = assigmentRepository.GetAssignments(userId);
            return result;
        }

        [HttpPost()]
        public void CreateAssignment([FromBody]CreateAssignmentInputModel assignment, [FromRoute] int userId)
        { 
            User assignator = userRepository.GetById(userId);
            Assignment assigmentUpdate = assigmentRepository.GetAssigmentExist(assignment.User.Id, assignment.HourType.Id, assignment.Project.Id);

            IEnumerable<AssignmentType> typesAssigment = assigmentTypeRepository.GetAll();
            Assignment assigmentAdd = null;

            if(assigmentUpdate == null)
            {
                if (assignment.HourType == null)
                {
                    assigmentAdd = new Assignment
                    {
                        ProjectId = assignment.Project.Id,
                        AssigneeId = assignment.User.Id,
                        Assignator = assignator,
                        AssignmentTypeId = typesAssigment.First(t => t.Name == "Delegar proyecto").Id,
                        Hours = assignment.Hours,
                        Date = DateTime.Now
                    };
                }
                else
                {
                    assigmentAdd = new Assignment
                    {
                        ProjectId = assignment.Project.Id,
                        AssigneeId = assignment.User.Id,
                        Assignator = assignator,
                        AssignmentTypeId = typesAssigment.First(t => t.Name == "Delegar proyecto").Id,
                        HourTypeId = assignment.HourType.Id,
                        Hours = assignment.Hours,
                        Date = DateTime.Now
                    };
                }

                assigmentRepository.Add(assigmentAdd);
            }
            else
            {
                assigmentUpdate.Hours += assignment.Hours;

                assigmentRepository.Update(assigmentUpdate);
            }  
        }

        //[HttpGet("Hours")]
        //public IEnumerable<Assignment> GetHours(int userId)
        //{
        //    IEnumerable<Assignment> result = assigmentRepository.GetAssignments(userId, AssignmentType.AssignmentTypeHours);
        //    return result;
        //}

     

    }
}
