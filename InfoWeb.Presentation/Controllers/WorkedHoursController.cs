using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using InfoWeb.Presentation.InputModels;
using InfoWeb.Presentation.Models;
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

                    var hourType = project.ProjectsHoursTypes.Where(pht => pht.HourType.Id == inputModel.HourType.Id).FirstOrDefault();
                    if (hourType != null)
                    {
                        var startDate = new DateTime();
                        var endTime = new DateTime();
                        bool startDateParseResult = DateTime.TryParse(inputModel.DateStart, out startDate);
                        bool endDateParseResult = DateTime.TryParse(inputModel.DateEnd, out endTime);

                        if (startDateParseResult == true && endDateParseResult == true)
                        {
                            if (startDateParseResult == true && endDateParseResult == true)
                            {
                                var hours = (int)(endTime - startDate).TotalHours;
                                if (hours <= 0)
                                {
                                    return BadRequest("La cantidad de horas suministrada no es válida");
                                }

                                var statistics = GetWorkedHoursInformation(project, hourType, userId);
                            

                                    if ((statistics.WorkedHours + hours) <= statistics.TotalHours)
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
                                            Message = $"Ha subido {hours} de {hourType.HourType.Name} al proyecto <strong>{project.Name}</strong>.",
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
                                            return BadRequest(new ValidationResult("Error interno del servidor."));
                                        }
                                    }
                                    else
                                    {
                                        return BadRequest(new ValidationResult("La cantidad de horas suministrada excede el total disponible"));
                                    }
                                

                            }
                            else
                            {
                                return BadRequest(new ValidationResult("Formatos de fecha incorrectos."));
                            }

                        }
                        else
                        {
                            return BadRequest(new ValidationResult("Formatos de fecha incorrectos."));
                        }
                    }
                }

            }
            return BadRequest(new ValidationResult("Error en los datos de entrada"));
        }

        private WorkedHoursStatistics GetWorkedHoursInformation(Project project, ProjectsHoursTypes hourType, int userId)
        {
            WorkedHoursStatistics result = new WorkedHoursStatistics();

            var assignments = assignmentRepository.Assignments.Where(a => a.AssigneeId == userId &&
                                                 a.ProjectId == project.Id &&
                                                 a.HourTypeId == hourType.HourTypeId &&
                                                 (a.AssignmentType.Name == "Asignar horas" || a.AssignmentType.Name == "Asignar a grupo"))
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
                                                                        a.HourTypeId == hourType.HourTypeId &&
                                                                        a.Assignator.Id == groupAssignment.Assignator.Id &&
                                                                        a.AssignmentTypeId == groupAssignment.AssignmentTypeId &&
                                                                        a.AssigneeId != userId &&
                                                                        a.Date == groupAssignment.Date)
                                                                        .Select(a => a.AssigneeId)
                                                                        .ToList();
                uploadedHours += workedHourRepository.WorkedHours.Where(wo => assigments.Contains(wo.UserId) &&
                                                                        wo.ProjectHourTypeId == hourType.Id)
                                                                        .Sum(wo => wo.Hours);
            }

            result.TotalHours = totalAvailableHours;
            result.WorkedHours = uploadedHours;

            return result;
        }
        [HttpGet("{searchValue?}")]
        public IEnumerable<WorkedHour> GetWorkedHours([FromRoute]int userId, [FromQuery]string searchValue = "")
        {
            return workedHourRepository.GetworkedHoursByUser(userId, searchValue);
        }

    }
}
