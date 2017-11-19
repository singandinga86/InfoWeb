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
                    TotalHours = projectHourType.Hours
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
                projectRepository.Remove(project);

                try {
                    unitOfwork.Commit();
                }
                catch (Exception e)
                {
                    return BadRequest(new ValidationResult("Error interno del servidor."));
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
                             .Include(a => a.Assignee)
                             .ThenInclude(a => a.Role)
                             .GroupBy(a => a.Assignee)
                             .ToDictionary(a => a.Key, a => a.ToList());

            return assignments;
        }

        private IEnumerable<Assignment> getAssignmentsMadeToUserInProject(int userId, int projectId)
        {
            return queryModel.Assignments.Where(a => a.Assignee.Id == userId && a.ProjectId == projectId)
                .ToList();
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
                    if (isProjectAlreadyInserted(projectInputModel.Name, projectTypeId,clientId) == false)
                    {
                        Project project = new Project()
                        {
                            ClientId = projectInputModel.Client.Id,
                            TypeId = projectInputModel.Type.Id,
                            Name = projectInputModel.Name
                        };
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

                if (targetProject != null && isProjectAlreadyInserted(project.Name,
                    project.Type.Id, project.Client.Id) == false)
                {
                    targetProject.Name = project.Name;
                    targetProject.TypeId = project.Type.Id;
                    targetProject.ClientId = project.Client.Id;

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
    }
}
