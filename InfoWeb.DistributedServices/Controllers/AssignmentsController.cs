using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using InfoWeb.DistributedServices.InputModels;
using System.Net;

namespace InfoWeb.DistributedServices.Controllers
{
    [Route("api/user/{userId}/Assignments")]
    public class AssignmentsController: Controller
    {
        private readonly IAssignmentRepository assignmentRepository;
        private readonly IAssignmentTypeRepository assigmentTypeRepository;
        private readonly IUserRepository userRepository;
        private readonly IHourTypeRepository hourTypeRepository;
        private readonly IProjectRepository projectRepository;

        public AssignmentsController(IAssignmentRepository assigmentRepository,
                                     IAssignmentTypeRepository assigmentTypeRepository,
                                     IUserRepository userRepository,
                                     IHourTypeRepository hourTypeRepository,
                                     IProjectRepository projectRepository)
        {
            this.assignmentRepository = assigmentRepository;
            this.assigmentTypeRepository = assigmentTypeRepository;
            this.userRepository = userRepository;
            this.hourTypeRepository = hourTypeRepository;
            this.projectRepository = projectRepository;
        }

        [HttpGet()]
        public IEnumerable<Assignment> GetAssignments([FromRoute]int userId)
        {
            IEnumerable<Assignment> result = assignmentRepository.GetAssignmentsAssignedTo(userId);
            return result;
        }

        [HttpPost()]
        public void CreateAssignment([FromBody]CreateAssignmentInputModel assignment, [FromRoute] int userId)
        { 
            User assignator = userRepository.GetById(userId);
            Assignment assigmentUpdate = assignmentRepository.GetAssigmentExist(assignment.User.Id, assignment.HourType.Id, assignment.Project.Id);

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

                assignmentRepository.Add(assigmentAdd);
            }
            else
            {
                assigmentUpdate.Hours += assignment.Hours;

                assignmentRepository.Update(assigmentUpdate);
            }  
        }

        [HttpPost("technician")]
        public void CreateTechnicianAssignment([FromBody]CreateAssignmentInputModel assignment, [FromRoute] int userId)
        {
            var assignator = userRepository.GetById(userId);
            User assignee = (assignment.User != null) ? userRepository.GetById(assignment.User.Id): null;
            Project project = (assignment.Project != null) ? projectRepository.GetById(assignment.Project.Id) : null;
            HourType hourType = (assignment.HourType != null) ? hourTypeRepository.GetById(assignment.HourType.Id): null;
            AssignmentType assignmentType = assigmentTypeRepository.GetByName("Asignar hora");
            int hours = assignment.Hours;

            if (assignee != null && assignee.Role.Name == "Technician"
                && assignator != null && project != null && hourType != null
                && assignmentType != null && hours > 0)
            {
                Assignment targetAssignment = assignmentRepository.GetAssigmentExist(assignment.User.Id, assignment.HourType.Id, assignment.Project.Id);

                if(targetAssignment == null)
                {
                    targetAssignment = new Assignment
                    {
                        Assignee = assignee,
                        Assignator = assignator,
                        AssignmentType = assignmentType,
                        HourType = hourType,
                        Date = DateTime.Now,
                        Project = project
                    };
                    assignmentRepository.Add(targetAssignment);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                }
                else
                {
                    if(targetAssignment.Assignator.Id == assignator.Id)
                    {
                        targetAssignment.Hours += hours;
                        assignmentRepository.Update(targetAssignment);
                        Response.StatusCode = (int)HttpStatusCode.Created;
                    }
                    else
                    {
                        Response.StatusCode = (int)HttpStatusCode.Conflict;
                    }                    
                }                
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

        }

        [HttpPost("project")]
        public void AssignProject([FromBody]AssignProjectInputModel model, [FromRoute]int userId)
        {
            var assignee = userRepository.GetById(model.AssigneeId);
            var assignator = userRepository.GetById(userId);
            var assignmentType = assigmentTypeRepository.GetByName("Delegar proyecto");
            var project = projectRepository.GetById(model.ProjectId);

            if (assignee != null && assignator != null &&
                assignee.Role.Name == "OM" && assignator.Role.Name == "Admin"
                && project != null && assignmentType != null)
            {
                var assignment = new Assignment()
                {
                    Assignee = assignee,
                    Assignator = assignator,
                    HourType = null,
                    AssignmentType = assignmentType,
                    Date = DateTime.Now,
                    Project = project,
                };

                assignmentRepository.Add(assignment);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
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
