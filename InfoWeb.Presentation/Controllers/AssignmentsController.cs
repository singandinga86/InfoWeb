﻿using System;
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
using InfoWeb.Domain.Comparers;

namespace InfoWeb.Presentation.Controllers
{
    [Route("api/user/{userId}/Assignments")]
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentRepository assignmentRepository;
        private readonly IAssignmentTypeRepository assigmentTypeRepository;
        private readonly IUserRepository userRepository;
        private readonly IHourTypeRepository hourTypeRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationRepository notificationRepository;
        private readonly IProjectHourTypeRepository projectHourTypeRepository;



        public AssignmentsController(IAssignmentRepository assigmentRepository,
                                     IAssignmentTypeRepository assigmentTypeRepository,
                                     IUserRepository userRepository,
                                     IHourTypeRepository hourTypeRepository,
                                     IProjectRepository projectRepository,
                                     IUnitOfWork unitOfWork,
                                     INotificationRepository notificationRepository,
                                     IProjectHourTypeRepository projectHourTypeRepository)
        {
            this.assignmentRepository = assigmentRepository;
            this.assigmentTypeRepository = assigmentTypeRepository;
            this.userRepository = userRepository;
            this.hourTypeRepository = hourTypeRepository;
            this.projectRepository = projectRepository;
            this.unitOfWork = unitOfWork;
            this.notificationRepository = notificationRepository;
            this.projectHourTypeRepository = projectHourTypeRepository;
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
            if (ModelState.IsValid)
            {
                User assignator = userRepository.GetById(userId);
                Assignment assigmentUpdate = null;
                HourType hourType = assignment.HourType != null ? hourTypeRepository.GetById(assignment.HourType.Id) : null;
                AssignmentType assignmentType = assigmentTypeRepository.GetByName("Delegar proyecto");
                AssignmentType assignmentTypeGroup = assigmentTypeRepository.GetByName("Asignar a grupo");
                User assignee = userRepository.GetById(assignment.User.Id);
                Project project = projectRepository.GetById(assignment.Project.Id);

                var existAssignment = assignmentRepository.Assignments
                                    .Where(a => a.AssignmentTypeId == assignmentTypeGroup.Id && a.ProjectId == project.Id).FirstOrDefault();



                if (assignator != null && assignee != null && assignmentType != null && project != null)
                {
                    Notification notification = null;

                    var validationResult = CanProjectDelegationBeDone(assignment, assignmentType, project, assignment.Hours == null?0 : assignment.Hours.Value);

                    if (validationResult.IsValid)
                    {

                        if (assignment.HourType != null)
                            assigmentUpdate = assignmentRepository.GetAssigmentExist(assignment.User.Id, assignment.HourType.Id, assignment.Project.Id);
                        else
                        {
                            if (existAssignment != null)
                                return BadRequest(new ValidationResult("Ya se han delegado horas a grupo del proyecto <strong>" + project.Name + "</strong>."));
                        }

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

                                assignmentRepository.removeAssigmentsAssignedTo(assignee.Id, project.Id, assignmentType.Id);
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
                                    Hours = assignment.Hours.Value,
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
                            assigmentUpdate.Hours += assignment.Hours.Value;

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

            return Ok($"El proycto <strong>{assignment.Project.Name}</strong> fue delegado correctamente.");
        }

        [HttpPost("technician")]
        public IActionResult CreateTechnicianAssignment([FromBody]CreateAssignmentInputModel assignment, [FromRoute] int userId)
        {
            var assignator = userRepository.GetById(userId);
            User assignee = (assignment.User != null) ? userRepository.GetById(assignment.User.Id) : null;
            Project project = (assignment.Project != null) ? projectRepository.GetById(assignment.Project.Id) : null;
            HourType hourType = (assignment.HourType != null) ? hourTypeRepository.GetById(assignment.HourType.Id) : null;
            AssignmentType assignmentType = assigmentTypeRepository.GetByName("Asignar horas");
            int hours = assignment.Hours.Value;

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
                            Message = "Se le han asignado " + assignment.Hours + " horas de " + hourType.Name + " en el proyecto " + project.Name,
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
            var assignee = userRepository.GetById(model.AssigneeId);
            var assignator = userRepository.GetById(userId);
            var assignmentType = assigmentTypeRepository.GetByName("Asignar proyecto");
            var project = projectRepository.GetById(model.ProjectId);

            if (model != null)
            {            

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

            return Ok($"El proycto <strong>{project.Name}</strong> fue asignado correctamente.");
        }

        [HttpPost("projectGroup")]
        public IActionResult AssignProjectGroup([FromBody]AssignProjectGroupInputModel model, [FromRoute]int userId)
        {
            /*if(model != null)
            {
                var date = DateTime.Now;
                var assignmentType = assigmentTypeRepository.GetByName("Asignar a grupo");

                var existAssignment = assignmentRepository.Assignments
                                      .Where(a => a.AssignmentTypeId == assignmentType.Id && a.ProjectId == model.project.Id 
                                      && a.HourTypeId == model.hourType.Id).FirstOrDefault();
                var existAssignmentbyHours = assignmentRepository.Assignments
                                    .Where(a => a.AssignmentType.Name == "Asignar horas" && a.ProjectId == model.project.Id
                                    && a.HourTypeId == model.hourType.Id).FirstOrDefault();
                if(existAssignmentbyHours ==  null)
                {
                    var userAssignatorAssigment = assignmentRepository.GetAssigmentUserAssginator(userId, model.project.Id);
                    if (existAssignment == null)
                    {

                        var assignmentToOM = assignmentRepository.Assignments.Where(a => a.ProjectId == model.project.Id && a.Assignator.Id == userId
                                             && a.AssignmentType.Name == "Delegar proyecto" && a.Assignator.Role.Name == "OM" && a.HourTypeId == null).FirstOrDefault();


                        if (assignmentToOM == null)
                        {
                            var assignmentToOMByHour = assignmentRepository.Assignments.Where(a => a.ProjectId == model.project.Id && a.Assignator.Id == userId
                                           && a.AssignmentType.Name == "Delegar proyecto" && a.Assignator.Role.Name == "OM" && a.HourTypeId == model.hourType.Id).FirstOrDefault();

                            if (assignmentToOMByHour == null)
                            {
                                if (userAssignatorAssigment.HourType != null)
                                {
                                    foreach (var user in model.usersSelected)
                                    {
                                        var assignment = new Assignment()
                                        {
                                            AssigneeId = user.Id,
                                            Assignator = userAssignatorAssigment.Assignee,
                                            AssignmentType = assignmentType,
                                            HourTypeId = userAssignatorAssigment.HourTypeId,
                                            Date = date,
                                            ProjectId = userAssignatorAssigment.ProjectId,
                                            Hours = userAssignatorAssigment.Hours
                                        };

                                        Notification notification = new Notification()
                                        {
                                            Date = DateTime.Now,
                                            Message = "Se le han asignado " + userAssignatorAssigment.Hours + " horas de " + model.hourType.Name + " a consumir del proyecto " + model.project.Name,
                                            Seen = false,
                                            UserId = user.Id,
                                            SenderId = userAssignatorAssigment.Assignee.Id
                                        };
                                        notificationRepository.Add(notification);

                                        assignmentRepository.Add(assignment);
                                    }
                                }
                                else
                                {
                                    var hourTypeProject = projectHourTypeRepository.GetHourType(model.project.Id, model.hourType.Id);
                                    foreach (var user in model.usersSelected)
                                    {
                                        var assignment = new Assignment()
                                        {
                                            AssigneeId = user.Id,
                                            Assignator = userAssignatorAssigment.Assignee,
                                            AssignmentType = assignmentType,
                                            HourTypeId = hourTypeProject.HourTypeId,
                                            Date = date,
                                            ProjectId = hourTypeProject.ProjectId,
                                            Hours = hourTypeProject.Hours
                                        };
                                        Notification notification = new Notification()
                                        {
                                            Date = DateTime.Now,
                                            Message = "Se le han asignado " + hourTypeProject.Hours + " horas de " + model.hourType.Name + " a consumir del proyecto " + model.project.Name,
                                            Seen = false,
                                            UserId = user.Id,
                                            SenderId = userAssignatorAssigment.Assignee.Id
                                        };
                                        notificationRepository.Add(notification);

                                        assignmentRepository.Add(assignment);
                                    }
                                }

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
                                return BadRequest(new ValidationResult("Ya se han delegado horas de este tipo."));

                        }
                        else
                            return BadRequest(new ValidationResult("Ya se delegó este proyecto anteriormente."));
                    }
                    else
                        return BadRequest(new ValidationResult("Ya se ha asignado estas horas a un grupo."));
                }
                else
                    return BadRequest(new ValidationResult("Ya se ha asignado horas de este tipo."));






            }
            else
                return BadRequest(new ValidationResult("Error en los datos de entrada."));

            return Ok();*/

            if(ModelState.IsValid)
            {
                var project = projectRepository.GetById(model.project.Id);
                var assignator = userRepository.GetById(userId);
                if(project != null && assignator != null)
                {
                    var targetHourType = project.ProjectsHoursTypes
                                         .Where(pht => pht.HourTypeId == model.hourType.Id)
                                         .FirstOrDefault();

                    if (targetHourType != null)
                    {
                        var assignmentType = assigmentTypeRepository.GetByName("Asignar a grupo");
                        var hourAssignmentType = assigmentTypeRepository.GetByName("Asignar horas");
                        //Find the parent assignment
                        var parentAssignment = assignmentRepository.Assignments
                                               .Where(a => a.AssigneeId == userId
                                               && a.ProjectId == project.Id &&
                                               (a.HourTypeId == model.hourType.Id || a.HourTypeId == null))
                                               .FirstOrDefault();
                        if (parentAssignment != null)
                        {
                            /*var hourCount = assignmentRepository.Assignments
                                            .Where(a => a.ProjectId == project.Id && a.AssignmentTypeId == hourAssignmentType.Id
                                            && a.HourTypeId == model.hourType.Id && a.Assignator.Id == userId)
                                            .Sum(a => a.Hours);
                            var groupAssignmentsMadeWithSameHourType = assignmentRepository.Assignments
                                            .Where(a => a.ProjectId == project.Id && a.AssignmentTypeId == assignmentType.Id)
                                            .ToList();
                            var groupAssignmentHourCount = groupAssignmentsMadeWithSameHourType
                                             .Distinct(new AssignmentByDateComparer())
                                             .Sum(a => a.Hours);

                            hourCount += groupAssignmentHourCount;*/

                            var totalHoursAssigned = assignmentRepository.Assignments
                                                    .Where(a => (a.HourTypeId == model.hourType.Id && a.AssignmentType.Name == "Asignar horas" && a.Assignator.Role.Name == "OM" && a.ProjectId == model.project.Id)
                                                    || (a.AssignmentType.Name == "Delegar proyecto" && a.HourTypeId == model.hourType.Id && a.ProjectId == model.project.Id))
                                                    .Sum(a => a.Hours);
                            var groupAssignmentsMadeWithSameHourType = assignmentRepository.Assignments
                                            .Where(a => a.ProjectId == model.project.Id && a.AssignmentType.Name == "Asignar a grupo"
                                            && a.HourTypeId == model.hourType.Id && a.Assignator.Role.Name == "OM")


                                            .ToList();
                            var groupAssignmentHourCount = groupAssignmentsMadeWithSameHourType
                                             .Distinct(new AssignmentByDateComparer()).Sum(a => a.Hours);


                            var hourCount = totalHoursAssigned + groupAssignmentHourCount;



                            var referenceHours = targetHourType.Hours;

                            if(parentAssignment.HourTypeId != null)
                            {
                                referenceHours = parentAssignment.Hours;
                            }

                            if((hourCount + model.Hours) <= referenceHours)
                            {
                                var date = DateTime.Now;
                                foreach (var user in model.usersSelected)
                                {
                                    var assignment = new Assignment()
                                    {
                                        AssigneeId = user.Id,
                                        Assignator = assignator,
                                        AssignmentType = assignmentType,
                                        HourTypeId = targetHourType.HourTypeId,
                                        Date = date,
                                        ProjectId = project.Id,
                                        Hours = model.Hours
                                    };
                                    Notification notification = new Notification()
                                    {
                                        Date = DateTime.Now,
                                        Message = "Se le han asignado " + model.Hours + " horas de " + targetHourType.HourType.Name + " a consumir del proyecto " + project.Name,
                                        Seen = false,
                                        UserId = user.Id,
                                        SenderId = assignator.Id
                                    };
                                    notificationRepository.Add(notification);

                                    assignmentRepository.Add(assignment);
                                }
                                try
                                {
                                    unitOfWork.Commit();
                                    return Ok("Asignación realizada correctamente.");
                                }
                                catch (Exception e)
                                {
                                    return BadRequest(new ValidationResult("Error interno del servidor."));
                                }
                            }
                            else
                            {
                                return BadRequest(new ValidationResult("La cantidad de horas de <strong>" + targetHourType.HourType.Name +"</strong> excede el máximo disponible." ));
                            }


                        }
                        else
                        {
                            return BadRequest(new ValidationResult("Usted no puede asignar el tipo de hora <strong>" + model.hourType.Name + "</strong> en el proyecto <strong>" + project.Name + "</strong>."));
                        }
                    }
                }
            }

            return BadRequest(new ValidationResult("Error en los datos de entrada."));

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
            //verificar se haya delegado el proyecto a ese mismo usuario
            var AdministratorsCount = assignmentRepository.Assignments
                    .Where(a => a.AssigneeId == assignment.User.Id && a.ProjectId == project.Id &&
                           a.AssignmentType.Id == assignmentType.Id && a.Hours == 0)
                    .Count();

            if (AdministratorsCount > 0)
            {
                result.Messages.Add($"Ya se ha delegado el proyecto <strong>{project.Name}</strong> a este usuario.");
                return result;
            }
            if (assignment.HourType == null)
            {
            //verificar se hayan delegado o asignado horas a otro usuario
                var otherAdministratorsCount = assignmentRepository.Assignments
                    .Where(a => a.AssigneeId != assignment.User.Id && a.ProjectId == project.Id &&
                           a.AssignmentType.Id == assignmentType.Id  && a.Hours > 0)
                    .Count();
            //verificar se haya delegado otro usuario
            var othersAdministratorsCount = assignmentRepository.Assignments
                  .Where(a => a.AssigneeId != assignment.User.Id && a.ProjectId == project.Id &&
                         a.AssignmentType.Id == assignmentType.Id && a.Hours == 0)
                  .Count();

            var otherAssignmentCount = assignmentRepository.Assignments
                  .Where(a => a.AssigneeId != assignment.User.Id && a.ProjectId == project.Id &&
                         a.AssignmentType.Name == "Asignar horas" && a.Hours > 0)
                  .Count();
            if(otherAssignmentCount > 0 && assignment.HourType == null)
            {
                result.Messages.Add($"Ya se han asignado horas del proyecto <strong>{project.Name}</strong> a otros usuarios.");
                return result;
            }

            if (othersAdministratorsCount > 0)
                {
                    result.Messages.Add($"Ya se ha delegado el proyecto <strong>{project.Name}</strong>a otros usuarios.");
                    return result;
                }
                if (otherAdministratorsCount > 0)
                {
                    result.Messages.Add($"Ya se han delegado horas del proyecto <strong>{project.Name}</strong> a otros usuarios.");
                    return result;
                }
            }                       
            else
            {
            if(assignment.HourType != null)
            {
                var hourType = assignment.HourType != null ? project.ProjectsHoursTypes.Where(pht => pht.HourTypeId == assignment.HourType.Id).FirstOrDefault() : null;
                if (hourType == null)
                {
                    result.Messages.Add($"El proyecto <strong>{project.Name}</strong>no cuenta con horas de este tipo.");
                }
                else
                {
                    if (hours != 0)
                    {
                        var totalHours = hourType.Hours;

                        /*var currentHours = assignmentRepository.Assignments
                                    .Where(a => a.AssignmentTypeId == assignmentType.Id && a.ProjectId == project.Id &&
                                             a.HourTypeId == hourType.HourTypeId)
                                    .Sum(a => a.Hours);*/
                        var currentHours = assignmentRepository.Assignments
                                .Where(a => (a.HourTypeId == hourType.HourTypeId && a.AssignmentType.Name == "Asignar horas" && a.Assignator.Role.Name == "OM" && a.ProjectId == project.Id)
                                || (a.AssignmentType.Name == "Delegar proyecto" && a.HourTypeId == hourType.HourTypeId && a.ProjectId == project.Id))
                                .Sum(a => a.Hours);

                        var developerGroupAssignment = this.assigmentTypeRepository.GetByName("Asignar a grupo");

                        /*var developerGroupHours = assignmentRepository.Assignments.Where(a => a.ProjectId == project.Id &&
                                                                          a.HourTypeId == hourType.HourTypeId &&
                                                                          a.AssignmentTypeId == developerGroupAssignment.Id)
                                                                          .Select(a => new { a.Date, a.Hours })
                                                                          .Distinct().Sum(o => o.Hours);*/
                        var groupAssignmentsMadeWithSameHourType = assignmentRepository.Assignments
                                              .Where(a => a.ProjectId == project.Id && a.AssignmentType.Name == "Asignar a grupo"
                                              && a.HourTypeId == hourType.HourTypeId && a.Assignator.Role.Name == "OM")
                                              .ToList();


                        var developerGroupHours = groupAssignmentsMadeWithSameHourType
                                         .Distinct(new AssignmentByDateComparer()).Sum(a => a.Hours);

                        if ((currentHours + developerGroupHours + hours) > totalHours)
                        {
                            result.Messages.Add($"La cantidad de horas de <strong>{hourType.HourType.Name}</strong> seleccionada excede el total de horas.");
                        }
                    }
                    else
                        result.Messages.Add("Datos de entrada no válidos.");

                }
            }
            


            }

            return result;
        }

        private ValidationResult CanTechnicianAssignationBeDone(User assignator, Project project, HourType hourType, int hours)
        {
            var result = new ValidationResult();

            var projectContainsHourType = project.ProjectsHoursTypes.Any(pht => pht.HourTypeId == hourType.Id);
            AssignmentType assignmentTypeAssigneHour = assigmentTypeRepository.GetByName("Asignar horas");
            AssignmentType assignmentTypeDelegateHour = assigmentTypeRepository.GetByName("Delegar proyecto");

            
            var delegateProjectComplete = assignmentRepository.Assignments
                                          .Where(a => a.AssignmentTypeId == assignmentTypeDelegateHour.Id && a.Hours == 0 && a.HourTypeId == null
                                          && a.ProjectId == project.Id && a.Assignator.Id == assignator.Id && assignator.Role.Name == "OM").FirstOrDefault();
            if(delegateProjectComplete == null)
            {
                if (projectContainsHourType)
                {
                    var parentAssignments = assignmentRepository.Assignments
                                           .Where(a => a.Assignee.Id == assignator.Id && a.ProjectId == project.Id /*&&
                                          a.AssignmentTypeId == 1*/)
                                           .ToList();
                    int parentAssignmentCount = parentAssignments.Count;

                    if (parentAssignmentCount > 0)
                    {
                        var assignments = assignmentRepository.Assignments
                                                .Where(a => a.Assignator.Id == assignator.Id && a.HourTypeId == hourType.Id
                                                       && a.ProjectId == project.Id && (a.AssignmentTypeId == assignmentTypeAssigneHour.Id || (a.AssignmentTypeId == assignmentTypeDelegateHour.Id && a.HourTypeId != null)))
                                                .ToList();
                        int assignmentCount = assignments.Count();
                        int totalHours = 0;
                        int hourCount = assignments.Sum(a => a.Hours);

                        if (parentAssignmentCount == 1 && parentAssignments.First().HourType == null)
                        {
                            totalHours = project.ProjectsHoursTypes.Where(pht => pht.HourTypeId == hourType.Id).First().Hours;
                        }
                        else
                        {
                            var parentAuthorizationAssignment = parentAssignments
                                                                .Where(a => a.HourTypeId == hourType.Id)
                                                                .FirstOrDefault();

                            if (parentAuthorizationAssignment == null)
                            {
                                result.Messages.Add("No tiene permitido asignar este tipo de horas en este proyecto.");
                                return result;
                            }

                            totalHours = parentAuthorizationAssignment.Hours;
                        }

                        var developerGroupAssignment = this.assigmentTypeRepository.GetByName("Asignar a grupo");

                        var developerGroupHours = assignmentRepository.Assignments.Where(a => a.ProjectId == project.Id &&
                                                                              a.HourTypeId == hourType.Id &&
                                                                              a.AssignmentTypeId == developerGroupAssignment.Id &&
                                                                              a.Assignator.Id == assignator.Id)
                                                                              .Select(a => new { a.Date, a.Hours })
                                                                              .Distinct().Sum(o => o.Hours);

                        if ((hourCount + developerGroupHours + hours) > totalHours)
                        {
                            result.Messages.Add("La cantidad de horas seleccionada excede el total de horas delegadas.");
                        }
                    }
                    else
                    {
                        result.Messages.Add("No tiene permitido asignar horas en este proyecto.");
                    }
                }
                else
                {
                    result.Messages.Add("Este proyecto no cuenta con horas de este tipo.");
                }
            }
            else
                result.Messages.Add("Este proyecto ya ha sido delegado.");
            
            return result;
        }



    }
}
