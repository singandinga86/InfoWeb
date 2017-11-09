using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using InfoWeb.Presentation.InputModels;
using System.Net;
using InfoWeb.Presentation.Models;

namespace InfoWeb.Presentation.Controllers
{
    [Route("api/user/{userId}/Assignments")]
    public class AssignmentsController: Controller
    {
        private readonly IAssignmentRepository assignmentRepository;
        private readonly IAssignmentTypeRepository assigmentTypeRepository;
        private readonly IUserRepository userRepository;
        private readonly IHourTypeRepository hourTypeRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationRepository notificationRepository;



        public AssignmentsController(IAssignmentRepository assigmentRepository,
                                     IAssignmentTypeRepository assigmentTypeRepository,
                                     IUserRepository userRepository,
                                     IHourTypeRepository hourTypeRepository,
                                     IProjectRepository projectRepository,
                                     IUnitOfWork unitOfWork,
                                     INotificationRepository notificationRepository)
        {
            this.assignmentRepository = assigmentRepository;
            this.assigmentTypeRepository = assigmentTypeRepository;
            this.userRepository = userRepository;
            this.hourTypeRepository = hourTypeRepository;
            this.projectRepository = projectRepository;
            this.unitOfWork = unitOfWork;
            this.notificationRepository = notificationRepository;
        }

        [HttpGet]
        public IEnumerable<Assignment> GetAssignments([FromRoute]int userId)
        {
            IEnumerable<Assignment> result = assignmentRepository.GetAssignmentsAssignedTo(userId);
            return result;
        }

        [HttpPost]
        public IActionResult CreateAssignment([FromBody]CreateAssignmentInputModel assignment, [FromRoute] int userId)
        { 
            if(ModelState.IsValid)
            {
                User assignator = userRepository.GetById(userId);
                Assignment assigmentUpdate = null;
                HourType hourType = hourTypeRepository.GetById(assignment.HourType.Id);
                AssignmentType assignmentType = assigmentTypeRepository.GetByName("Delegar proyecto");
                User assignee = userRepository.GetById(assignment.User.Id);
                Project project = projectRepository.GetById(assignment.Project.Id);                

                if (assignator != null && assignee != null && assignmentType != null && project != null && assignment.Hours > 0)
                {
                    Notification notification = null;

                    var validationResult = CanProjectDelegationBeDone(assignment, assignmentType, project, assignment.Hours);

                    if (validationResult.IsValid)
                    {

                        if (assignment.HourType != null)
                            assigmentUpdate = assignmentRepository.GetAssigmentExist(assignment.User.Id, assignment.HourType.Id, assignment.Project.Id);

                        Assignment assigmentAdd = null;

                        if (assigmentUpdate == null)
                        {
                            if (assignment.HourType == null)
                            {
                                assigmentAdd = new Assignment
                                {
                                    ProjectId = assignment.Project.Id,
                                    AssigneeId = assignment.User.Id,
                                    Assignator = assignator,
                                    AssignmentType = assignmentType,
                                    Hours = assignment.Hours,
                                    Date = DateTime.Now
                                };
                                notification = new Notification()
                                {
                                    Date = DateTime.Now,
                                    Message = "Se le ha delegado el proyecto " + assignment.Project.Name,
                                    Seen = false,
                                    UserId = assignment.User.Id,
                                    SenderId = assignator.Id
                                };

                                assignmentRepository.removeAssigmentsAssignedTo(assignee.Id, project.Id, hourType.Id, assignmentType.Id);
                            }
                            else
                            {
                                assigmentAdd = new Assignment
                                {
                                    ProjectId = assignment.Project.Id,
                                    AssigneeId = assignment.User.Id,
                                    Assignator = assignator,
                                    AssignmentType = assignmentType,
                                    HourTypeId = assignment.HourType.Id,
                                    Hours = assignment.Hours,
                                    Date = DateTime.Now
                                };
                                notification = new Notification()
                                {
                                    Date = DateTime.Now,
                                    Message = "Se le ha delegado el proyecto " + assignment.Project.Name + " con " + assignment.Hours + " horas de " + assignment.HourType.Name,
                                    Seen = false,
                                    UserId = assignment.User.Id,
                                    SenderId = assignator.Id
                                };
                            }
                            notificationRepository.Add(notification);
                            assignmentRepository.Add(assigmentAdd);
                        }
                        else
                        {
                            assigmentUpdate.Hours += assignment.Hours;

                            assignmentRepository.Update(assigmentUpdate);
                        }

                        try
                        {
                            unitOfWork.Commit();
                            Response.StatusCode = (int)HttpStatusCode.Created;

                        }
                        catch (Exception e)
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        return BadRequest(validationResult);
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("Datos de entrada no válidos."));
                }

            }
            else
            {
                return BadRequest(new ValidationResult("Datos de entrada no válidos."));
            }

            return Ok();            
        }

        [HttpPost("technician")]
        public IActionResult CreateTechnicianAssignment([FromBody]CreateAssignmentInputModel assignment, [FromRoute] int userId)
        {
            var assignator = userRepository.GetById(userId);
            User assignee = (assignment.User != null) ? userRepository.GetById(assignment.User.Id): null;
            Project project = (assignment.Project != null) ? projectRepository.GetById(assignment.Project.Id) : null;
            HourType hourType = (assignment.HourType != null) ? hourTypeRepository.GetById(assignment.HourType.Id): null;
            AssignmentType assignmentType = assigmentTypeRepository.GetByName("Asignar horas");
            int hours = assignment.Hours;

            if (assignee != null && assignee.Role.Name == "TEC"
                && assignator != null && project != null && hourType != null
                && assignmentType != null && hours > 0)
            {

                Assignment targetAssignment = assignmentRepository.GetAssigmentExist(assignment.User.Id, assignment.HourType.Id, assignment.Project.Id);

                var validationResult = CanTechnicianAssignationBeDone(assignator, project, hourType, hours);

                if (validationResult.IsValid)
                {

                    if (targetAssignment == null)
                    {
                        targetAssignment = new Assignment
                        {
                            Assignee = assignee,
                            Assignator = assignator,
                            AssignmentType = assignmentType,
                            HourType = hourType,
                            Date = DateTime.Now,
                            Project = project,
                            Hours = hours
                        };
                        assignmentRepository.Add(targetAssignment);
                        Response.StatusCode = (int)HttpStatusCode.Created;
                    }
                    else
                    {
                        if (targetAssignment.Assignator.Id == assignator.Id)
                        {
                            targetAssignment.Hours += hours;
                            assignmentRepository.Update(targetAssignment);
                            Response.StatusCode = (int)HttpStatusCode.Created;
                        }
                        else
                        {
                            return BadRequest(new ValidationResult("Otro usuario ya efectuó esta asignación."));
                        }
                    }

                    if (Response.StatusCode != (int)HttpStatusCode.Conflict)
                    {
                        Notification notification = new Notification() {
                            Date = DateTime.Now,
                            Message = "Se le han asignado " + assignment.Hours + " horas de " + hourType.Name +" en el proyecto " + project.Name,
                            Seen = false,
                            UserId = assignee.Id,
                            SenderId = assignator.Id
                        };

                        notificationRepository.Add(notification);

                        try
                        {
                            unitOfWork.Commit();
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                }
                else
                {
                    return BadRequest(validationResult);
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok();

        }

        [HttpPost("project")]
        public IActionResult AssignProject([FromBody]AssignProjectInputModel model, [FromRoute]int userId)
        {
            if (model != null)
            {
                var assignee = userRepository.GetById(model.AssigneeId);
                var assignator = userRepository.GetById(userId);
                var assignmentType = assigmentTypeRepository.GetByName("Asignar proyecto");
                var project = projectRepository.GetById(model.ProjectId);

                if (assignee != null && assignator != null &&
                    assignee.Role.Name == "OM" && assignator.Role.Name == "ADMIN"
                    && project != null && assignmentType != null)
                {
                    var targetAssignment = assignmentRepository.Assignments
                    .Where(a =>/* a.Assignee.Id == assignee.Id && */a.Project.Id == project.Id
                           && a.AssignmentType.Id == assignmentType.Id && a.HourType == null)
                    .FirstOrDefault();

                    if (targetAssignment == null)
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

                        Notification notification = new Notification()
                        {
                            Date = DateTime.Now,
                            Message = "Se le ha asignado el proyecto " + project.Name,
                            Seen = false,
                            UserId = assignee.Id,
                            SenderId = assignator.Id
                        };
                        notificationRepository.Add(notification);

                        try
                        {
                            unitOfWork.Commit();
                            Response.StatusCode = (int)HttpStatusCode.Created;
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("Este proyecto ya fue asignado."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("Error en los datos de entrada."));
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));   
            }

            return Ok();
        }

        //[HttpGet("Hours")]
        //public IEnumerable<Assignment> GetHours(int userId)
        //{
        //    IEnumerable<Assignment> result = assigmentRepository.GetAssignments(userId, AssignmentType.AssignmentTypeHours);
        //    return result;
        //}

        private ValidationResult CanProjectDelegationBeDone(CreateAssignmentInputModel assignment, AssignmentType assignmentType, Project project, int hours)
        {
            var result = new ValidationResult();
            if(assignment.HourType == null)
            {
                var otherAdministratorsCount = assignmentRepository.Assignments
                    .Where(a => a.AssigneeId != assignment.User.Id && a.ProjectId == project.Id &&
                           a.AssignmentType.Id == assignmentType.Id)
                    .Count();
                if(otherAdministratorsCount > 0)
                {
                    result.Messages.Add("Ya se le han delegado horas de este proyecto a otros usuarios.");
                }
            }
            else
            {
                var hourType = project.ProjectsHoursTypes.Where(pht => pht.Id == assignment.HourType.Id).FirstOrDefault();
                if(hourType == null)
                {
                    result.Messages.Add("Este proyecto no cuenta con horas de este tipo.");
                }

                var totalHours = hourType.Hours;

                var currentHours = assignmentRepository.Assignments
                            .Where(a => a.AssignmentType.Id == assignmentType.Id && a.ProjectId == project.Id &&
                                     a.HourTypeId == hourType.Id)
                            .Sum(a => a.Hours);

                if((currentHours + hours) > totalHours)
                {
                    result.Messages.Add("La cantidad de horas seleccionada excecde el total de horas del proyecto.");
                }
                
            }

            return result;
        }

        private ValidationResult CanTechnicianAssignationBeDone(User assignator, Project project, HourType hourType, int hours)
        {
            var result = new ValidationResult();
            var parentAssignments = assignmentRepository.Assignments
                                   .Where(a => a.Assignee.Id == assignator.Id && a.ProjectId == project.Id /*&&
                                          a.AssignmentTypeId == 1*/)
                                   .ToList();
            int parentAssignmentCount = parentAssignments.Count;

            if(parentAssignmentCount > 0)
            {
                var assignments = assignmentRepository.Assignments
                                        .Where(a => a.Assignator.Id == assignator.Id && a.HourTypeId == hourType.Id 
                                               && a.ProjectId == project.Id && a.AssignmentTypeId == 2)
                                        .ToList();
                int assignmentCount = assignments.Count();
                int totalHours = 0;
                int hourCount = assignments.Sum(a => a.Hours);

                if (parentAssignmentCount == 1 && parentAssignments.First().HourType == null)
                {
                    totalHours = project.ProjectsHoursTypes.Where(pht => pht.Id == hourType.Id).First().Hours;
                }
                else
                {
                    var parentAuthorizationAssignment = parentAssignments
                                                        .Where(a => a.HourTypeId == hourType.Id)
                                                        .FirstOrDefault();

                    if(parentAuthorizationAssignment == null)
                    {
                        result.Messages.Add("No tiene permitido asignar este tipo de horas en este proyecto.");
                    }

                    totalHours = parentAuthorizationAssignment.Hours;
                }

                if ((hourCount + hours) > totalHours)
                {
                    result.Messages.Add("La cantidad de horas seleccionada excecde el total de horas del proyecto.");
                }
            }
            else
            {
                result.Messages.Add("No tiene permitido asignar horas en este proyecto.");
            }

            return result;
        }



    }
}
