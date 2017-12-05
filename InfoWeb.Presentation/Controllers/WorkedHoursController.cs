﻿using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using InfoWeb.Presentation.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.Presentation.Controllers
{
    [Route("api/user/{userId}/WorkedHours")]
    public class WorkedHoursController: Controller
    {
        private readonly IAssignmentRepository assignmentRepository;
        //private readonly IHourTypeRepository hourTypeRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly INotificationRepository notificationRepository;
        private readonly IProjectHourTypeRepository projectHourTypeRepository;
        private readonly IWorkedHourRepository workedHourRepository;



        public WorkedHoursController(IAssignmentRepository assigmentRepository,
                                     /*IHourTypeRepository hourTypeRepository,*/
                                     IProjectRepository projectRepository,
                                     IUnitOfWork unitOfWork,
                                     INotificationRepository notificationRepository,
                                     IProjectHourTypeRepository projectHourTypeRepository,
                                     IWorkedHourRepository workedHourRepository)
        {
            this.assignmentRepository = assigmentRepository;
            //this.hourTypeRepository = hourTypeRepository;
            this.projectRepository = projectRepository;
            this.unitOfWork = unitOfWork;
            this.notificationRepository = notificationRepository;
            this.projectHourTypeRepository = projectHourTypeRepository;
            this.workedHourRepository = workedHourRepository;
        }

        [HttpPost]
        public IActionResult UploadHours([FromBody]UploadHoursInputModel inputModel, [FromRoute] int userId)
        {
            if (ModelState.IsValid)
            {
                var project = projectRepository.GetById(inputModel.Project.Id);
                if (project != null)
                {
                    var hourType = projectHourTypeRepository.GetById(inputModel.ProjectHourType.Id);
                    if (hourType != null)
                    {
                        var startDate = new DateTime();
                        var endTime = new DateTime();
                        bool startDateParseResult = DateTime.TryParse(inputModel.StartDate, out startDate);
                        bool endDateParseResult = DateTime.TryParse(inputModel.EndDate, out endTime);

                        if (startDateParseResult == true && endDateParseResult == true)
                        {
                            if (startDateParseResult == true && endDateParseResult == true)
                            {
                                var hours = (endTime - startDate).Hours;
                                if (hours < 0)
                                {
                                    return BadRequest("La cantidad de horas suministrada no es válida");
                                }

                                var assignments = assignmentRepository.Assignments.Where(a => a.AssigneeId == userId &&
                                                  a.ProjectId == project.Id &&
                                                  a.HourTypeId == hourType.Id &&
                                                  (a.HourType.Name == "Asignar horas" || a.HourType.Name == "Asignar a grupo"))
                                                  .Include(a => a.AssignmentType)
                                                  .Include(a => a.Assignator)
                                                  .ToList();

                                var hourAssignment = assignments.Where(a => a.AssignmentType.Name == "Asignar horas").FirstOrDefault();
                                int totalAvailableHours = 0;
                                if (hourAssignment != null)
                                {
                                    totalAvailableHours += hourAssignment.Hours;
                                }
                                int uploadedHours = workedHourRepository.WorkedHours.Where(wo => wo.UserId == userId &&
                                                                                            wo.ProjectHourTypeId == hourType.Id)
                                                                                           .Sum(wo => wo.Hours);

                                var groupAssignment = assignments.Where(a => a.AssignmentType.Name == "Asignar a grupo").FirstOrDefault();

                                if (groupAssignment != null)
                                {
                                    totalAvailableHours += groupAssignment.Hours;
                                    var assigments = assignmentRepository.Assignments.Where(a => a.ProjectId == project.Id &&
                                                                                            a.HourTypeId == hourType.Id &&
                                                                                            a.Assignator.Id == groupAssignment.Assignator.Id &&
                                                                                            a.HourType.Name == "Asignar a grupo" &&
                                                                                            a.AssigneeId != userId &&
                                                                                            a.Date == groupAssignment.Date)
                                                                                            .Select(a => a.AssigneeId)
                                                                                            .ToList();
                                    uploadedHours += workedHourRepository.WorkedHours.Where(wo => assigments.Contains(wo.UserId) &&
                                                                                            wo.ProjectHourTypeId == hourType.Id)
                                                                                            .Sum(wo => wo.Hours);
                                }
                            

                                    if ((uploadedHours + hours) <= totalAvailableHours)
                                    {
                                        var workedHour = new WorkedHour()
                                        {
                                            Description = inputModel.Description,
                                            Hours = hours,
                                            ProjectHourTypeId = hourType.Id,
                                            UserId = userId
                                        };
                                        var notification = new Notification()
                                        {
                                            Date = DateTime.Now,
                                            Message = $"Ha subido {hours} de {hourType.HourType.Name} al proyecto {project.Name}.",
                                            Seen = false,
                                            SenderId = userId,
                                            UserId = userId
                                        };
                                        workedHourRepository.Add(workedHour);
                                        notificationRepository.Add(notification);

                                        try
                                        {
                                            unitOfWork.Commit();
                                            return Ok($"Ha subido {hours} de {hourType.HourType.Name} al proyecto {project.Name}.");
                                        }
                                        catch (Exception e)
                                        {
                                            return BadRequest("Error interno del servidor.");
                                        }
                                    }
                                    else
                                    {
                                        return BadRequest("La cantidad de horas suministrada excede el total disponible");
                                    }
                                

                            }
                            else
                            {
                                return BadRequest("Formatos de fecha incorrectos.");
                            }

                        }
                        else
                        {
                            return BadRequest("Formatos de fecha incorrectos.");
                        }
                    }
                }

            }
            return BadRequest("Error en los datos de entrada");
        }

    }
}
