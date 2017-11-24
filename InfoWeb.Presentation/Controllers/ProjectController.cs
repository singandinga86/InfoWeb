using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using InfoWeb.Presentation.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using InfoWeb.Presentation.InputModels;
using InfoWeb.Domain.Comparers;

namespace InfoWeb.Presentation.Controllers
{
    [Route("api/{userId}/Projects")]
    public class ProjectController : Controller
    {
        private IProjectRepository projectRepository;
        private IUserRepository userRepository;
        private IProjectHourTypeRepository projectHourTypeRepository;
        private IInfoWebQueryModel queryModel;
        private readonly IUnitOfWork unitOfwork;
        public ProjectController(IProjectRepository projectRepository,
                                    IUserRepository userRepository,
                                    IUnitOfWork unitOfwork,
                                    IInfoWebQueryModel queryModel,
                                    IProjectHourTypeRepository projectHourTypeRepository)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.projectHourTypeRepository = projectHourTypeRepository;
            this.queryModel = queryModel;
            this.unitOfwork = unitOfwork;
        }

        [HttpGet]
        public IActionResult GetProjectsForUser([FromRoute]int userId)
        {
            var user = userRepository.GetById(userId);
            IEnumerable<Project> projects = null;
            if (user != null)
            {
                if (user.Role.Name == "ADMIN")
                {
                    projects = projectRepository.GetAll();
                }
                else
                {
                    projects = projectRepository.GetProjectsAssignedTo(userId);
                }
                foreach (var p in projects)
                {
                    p.Client.Projects = null;
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Usuario no válido"));
            }
            return Ok(projects);
        }

        [HttpGet("AssignedBy")]
        public IEnumerable<Project> GetProjectsAssignedBy(int userId)
        {
            return projectRepository.GetProjectsAssignedBy(userId);
        }

        [HttpGet("AssignedTo")]
        public IEnumerable<Project> GetProjectsAssignedTo(int userId)
        {
            return projectRepository.GetProjectsAssignedTo(userId);
        }

        [HttpGet("{projectId}/details")]
        public ProjectDetailsViewModel GetProjectDetails(int userId, int projectId)
        {
            ProjectDetailsViewModel result = new ProjectDetailsViewModel();
            var user = userRepository.GetById(userId);
            var project = projectRepository.GetById(projectId);
            var projectHoursTypes = (List<ProjectsHoursTypes>)projectHourTypeRepository.GetListById(projectId);
            result.User = user;
            result.Project = project;

            if (user != null && project != null)
            {
                result.Details = getProjectDetailsforUser(user, projectId);
                result.Assignments = getAssignmentsMadeToUserInProject(userId, projectId);
            }
            List<ProjectHourDetailsViewModel> detailsHours = new List<ProjectHourDetailsViewModel>();
            foreach(var projectHourType in projectHoursTypes)
            {
                detailsHours.Add(new ProjectHourDetailsViewModel
                {
                    HourTypeName = projectHourType.HourType.Name,
                    ProjectName = projectHourType.Project.Name,
                    TotalHours = projectHourType.Hours,
                    TotalHoursAssigned = GetTotalHoursAssigned(projectHourType.HourType.Id, projectId)
                });
            }

            result.HoursDetails = detailsHours;
            result.Project.Assignments = null;

            return result;
        }

        [HttpGet("getUnassignedProjects")]
        public IEnumerable<Project> GetUnassignedProjects()
        {
            var result = projectRepository.GetUnassignedProjects();
            foreach (var p in result)
            {
                p.Client.Projects = null;
            }
            return result;
        }

        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute] int id)
        {
            var project = projectRepository.GetById(id);
            if (project != null)
            {
                if(projectRepository.CanItemBeRemoved(project.Id))
                {
                    projectRepository.Remove(project);

                    try
                    {
                        unitOfwork.Commit();
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new ValidationResult("Error interno del servidor."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("No se puede elimiar el proyecto. Tiene asignaciones asociadas a él."));
                }                
            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok();
        }

        private IEnumerable<ProjectDetailsUserAssigmentViewModel> getProjectDetailsforUser(User user, int projectId)
        {
            //ProjectDetailsViewModel result = new ProjectDetailsViewModel();
            var result = new List<ProjectDetailsUserAssigmentViewModel>();

            var assignmentsByUser = getAssignmentsMadeByUserInProject(user.Id, projectId);

            foreach (var assingmentCollection in assignmentsByUser)
            {
                var details = new ProjectDetailsUserAssigmentViewModel();
                details.User = assingmentCollection.Key;
                details.Assignments = assingmentCollection.Value;

                var roleName = details.User.Role.Name.ToLower();

                if (roleName.Contains("technician") == false)
                {
                    details.InnerAssignments = getProjectDetailsforUser(details.User, projectId);
                }

                result.Add(details);
            }

            return result;
        }

        private IDictionary<User, List<Assignment>> getAssignmentsMadeByUserInProject(int userId, int projectId)
        {
            var assignments = queryModel.Assignments.
                             Where(a => a.Assignator.Id == userId && a.Project.Id == projectId)
                             .Include(a => a.HourType)
                             .Include(a => a.AssignmentType)
                             .Include(a => a.Assignee)
                             .ThenInclude(a => a.Role)
                             .GroupBy(a => a.Assignee)
                             .ToDictionary(a => a.Key, a => a.ToList());

            return assignments;
        }

        private IEnumerable<Assignment> getAssignmentsMadeToUserInProject(int userId, int projectId)
        {
            return queryModel.Assignments
                   .Where(a => a.Assignee.Id == userId && a.ProjectId == projectId)
                   .Include(a => a.AssignmentType)
                   .ToList();
        }

        public int GetTotalHoursAssigned(int idHourtype, int idProject)
        {
            int totalHoursAssigned = 0;

            //contar horas asignadas por OM y delegadas por horas
            totalHoursAssigned = queryModel.Assignments
                                .Where(a => (a.HourTypeId == idHourtype && a.AssignmentType.Name == "Asignar horas" && a.Assignator.Role.Name == "OM" && a.ProjectId == idProject)
                                || (a.AssignmentType.Name == "Delegar proyecto" && a.HourTypeId == idHourtype && a.ProjectId == idProject))
                                .Sum(a => a.Hours);

            //contar horas asignadas a grupos de desarrolladores
            IEnumerable<int> projectsAssignedGroup = queryModel.Assignments
                                        .Where(a => a.ProjectId == idProject && a.AssignmentType.Name == "Asignar a grupo" && a.HourTypeId == idHourtype).Select(a => a.ProjectId).Distinct().ToList();
            if(projectsAssignedGroup.Count<int>() > 0)
            {
                projectsAssignedGroup.ToList().ForEach(p =>
               {
                   totalHoursAssigned += queryModel.ProjectHoursTypes
                                        .Where(pht => pht.ProjectId == p && pht.HourTypeId == idHourtype).Select(pht => pht.Hours).FirstOrDefault();
               });
            }

            //contar horas asignadas a grupos por TAM y PM
            var listAssignmentbyHours = queryModel.Assignments
                                  .Where(a => a.AssignmentType.Name == "Asignar horas" && a.ProjectId == idProject && a.HourTypeId == idHourtype
                                  && (a.Assignator.Role.Name == "PM" || a.Assignator.Role.Name == "TAM")).ToList();

            listAssignmentbyHours.ForEach(l =>
            {
                var assigmentParent = queryModel.Assignments
                                      .Where(a => a.AssigneeId == l.Assignator.Id && a.AssignmentType.Name == "Delegar proyecto"
                                      && a.ProjectId == idProject && a.HourTypeId == null && a.Hours == 0).FirstOrDefault();
                if (assigmentParent != null)
                    totalHoursAssigned += l.Hours;
                                        
            });

            return totalHoursAssigned;
        }

        [HttpGet("search/{searchValue}")]
        public IEnumerable<Project> searchProject([FromRoute]string searchValue)
        {
            return projectRepository.SearchProject(searchValue);

        }

        [HttpPost]
        public IActionResult Create([FromBody]ProjectInputModel projectInputModel)
        {
            if (ModelState.IsValid)
            {
                var clientId = projectInputModel.Client.Id;
                var projectTypeId = projectInputModel.Type.Id;


                var projectType = queryModel.ProjectType.Where(p => p.Id == projectTypeId).FirstOrDefault();
                var client = queryModel.Clients.Where(c => c.Id == clientId);

                if (projectType != null && client != null)
                {                   
                    if (isProjectAlreadyInserted(projectInputModel.Name, projectTypeId,clientId) == false &&
                        projectContainsValidHourTypes(projectInputModel.ProjectsHoursTypes) == true)
                    {
                        Project project = new Project()
                        {
                            ClientId = projectInputModel.Client.Id,
                            TypeId = projectInputModel.Type.Id,
                            Name = projectInputModel.Name,
                            ProjectsHoursTypes = new List<ProjectsHoursTypes>()
                        };

                        foreach(var pht in projectInputModel.ProjectsHoursTypes)
                        {
                            project.ProjectsHoursTypes.Add(new ProjectsHoursTypes() {
                                HourTypeId = pht.HourType.Id,
                                Hours = pht.Hours
                            });
                        }

                        projectRepository.Add(project);
                        try
                        {
                            unitOfwork.Commit();
                            return Ok();
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("Este proyecto ya existe."));
                    }
                }
            }

            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpPut]
        public IActionResult Update([FromBody]Project project)
        {
            if (ModelState.IsValid)
            {
                var targetProject = projectRepository.GetById(project.Id);

                if (targetProject != null)
                {
                    bool isProjectNameValid = true;
                    if (targetProject.Name.Trim().ToLower() != project.Name.Trim().ToLower())
                    {
                        isProjectNameValid = isProjectAlreadyInserted(project.Name, targetProject.TypeId, targetProject.ClientId);
                    }

                    if (isProjectNameValid)
                    {
                        if (projectContainsValidHourTypes(project.ProjectsHoursTypes) == true)
                        {
                            if (hourTypesContainValidHourAmounts(targetProject, project.ProjectsHoursTypes).IsValid)
                            {
                                var hourTypesToBeRemoved = targetProject.ProjectsHoursTypes.Except(project.ProjectsHoursTypes,
                                                            new ProjectHoursTypesByHourTypeComprarer()).ToList();
                                var hourTypeValidationResult = this.canProjectHourTypesBeReplaced(hourTypesToBeRemoved);

                                if (hourTypeValidationResult.IsValid)
                                {

                                    targetProject.Name = project.Name;
                                    targetProject.TypeId = project.Type.Id;
                                    targetProject.ClientId = project.Client.Id;

                                    var hourTypesToBeAdded = project.ProjectsHoursTypes.Except(targetProject.ProjectsHoursTypes,
                                                             new ProjectHoursTypesByHourTypeComprarer()).ToList();

                                    foreach (var hourType in hourTypesToBeRemoved)
                                    {
                                        targetProject.ProjectsHoursTypes.Remove(hourType);
                                    }                                  

                                    foreach (var pht in hourTypesToBeAdded)
                                    {
                                        targetProject.ProjectsHoursTypes.Add(new ProjectsHoursTypes
                                        {
                                            HourTypeId = pht.HourType.Id,
                                            Hours = pht.Hours
                                        });
                                    }

                                    projectRepository.Update(targetProject);
                                    try
                                    {
                                        unitOfwork.Commit();
                                    }
                                    catch (Exception e)
                                    {
                                        return BadRequest(new ValidationResult("Error interno del servidor."));
                                    }
                                }
                                else
                                {
                                    return BadRequest(hourTypeValidationResult.Messages.First());
                                }
                            }
                            else
                            {
                                return BadRequest(new ValidationResult("Cantidades de hora no válidos."));
                            }
                        }
                        else
                        {
                            return BadRequest(new ValidationResult("Tipos de hora no válidos."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("Este nombre de proyecto con el tipo seleccionado ya existe para este cliente."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("Proyecto no encontrado."));
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok();
        }

        [HttpGet("{projectId}/getProject")]
        public Project GetProject(int projectId)
        {
            return projectRepository.GetById(projectId);
        }

        private bool isProjectAlreadyInserted(string name, int projectTypeId, int clientId)
        {
            var targetProject = queryModel.Projects.Where(p => p.Name.ToLower() == name
                                                            && p.ClientId == clientId
                                                            && p.TypeId == projectTypeId).FirstOrDefault();
            if(targetProject != null)
            {
                return true;
            }
            return false;
        }

        private bool projectContainsValidHourTypes(IEnumerable<ProjectsHoursTypes> projectHourTypes)
        {
            if (projectHourTypes.Count() > 0)
            {
                var ids = projectHourTypes.Select(pht => pht.HourType.Id).Distinct().ToList();
                var hourTypeCount = projectHourTypes.Count();

                if (ids.Count() == hourTypeCount)
                {
                    var items = queryModel.HourTypes.Where(ht => ids.Contains(ht.Id)).Count();
                    if (items == hourTypeCount)
                    {
                        var invalidHours = projectHourTypes.Any(pht => pht.Hours < 1);
                        if (invalidHours == false)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        private ValidationResult hourTypesContainValidHourAmounts(Project project, IEnumerable<ProjectsHoursTypes> newHourTypes)
        {
            var result = new ValidationResult();
            foreach(var pht in project.ProjectsHoursTypes)
            {
                var targetHourType = newHourTypes.Where(ht => ht.HourType.Id == pht.HourTypeId).FirstOrDefault();
                if(targetHourType == null)
                {
                    var firstAssignment = queryModel.Assignments.Where(a => a.ProjectId == project.Id && a.HourTypeId == pht.HourTypeId).FirstOrDefault();
                    if(firstAssignment != null)
                    {
                        result.Messages.Add("El tipo de hora " + pht.HourType.Name + " no puede ser eliminado.");
                    }
                }
                else
                {
                    var targetHourTypeAssignmentCount = queryModel.Assignments
                                                   .Where(a => a.ProjectId == project.Id && a.HourTypeId == pht.HourTypeId)
                                                   .Count();

                    if (targetHourTypeAssignmentCount > pht.Hours)
                    {
                        result.Messages.Add("La cantidad de horas para " + pht.HourType.Name + "es menor que el total consumido");
                    }
                }
            }

            return result;
        }

        private ValidationResult canProjectHourTypesBeReplaced(IEnumerable<ProjectsHoursTypes> itemsToBeRemoved)
        {
            var result = new ValidationResult();



            foreach(var item in itemsToBeRemoved)
            {
                if(projectHourTypeRepository.CanItemBeRemoved(item.Id,item.ProjectId) == false)
                {
                    result.Messages.Add("El tipo de hora " + item.HourType.Name + " no puede ser eliminado porque tiene asigmaciones asociadas a él.");
                    return result;
                }
            }

            return result;
        }
    }
}
