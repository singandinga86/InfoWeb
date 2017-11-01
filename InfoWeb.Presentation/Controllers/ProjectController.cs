using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using InfoWeb.DistributedServices.Models;
using System.Net;
using Microsoft.EntityFrameworkCore;
using InfoWeb.DistributedServices.InputModels;
using InfoWeb.Presentation.InputModels;

namespace InfoWeb.DistributedServices.Controllers
{
    [Route("api/{userId}/Projects")]
    public class ProjectController : Controller
    {
        private IProjectRepository projectRepository;
        private IUserRepository userRepository;
        private IInfoWebQueryModel queryModel;
        public ProjectController(IProjectRepository projectRepository,
                                    IUserRepository userRepository,
                                    IAssignmentRepository assignmentRepository,
                                    IInfoWebQueryModel queryModel)
        {
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.queryModel = queryModel;
        }

        [HttpGet]
        public IEnumerable<Project> GetProjectsForUser([FromRoute]int userId)
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
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return projects;
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
            result.User = user;
            result.Project = project;

            if (user != null && project != null)
            {
                result.Details = getProjectDetailsforUser(user, projectId);
                result.Assignments = getAssignmentsMadeToUserInProject(userId, projectId);
            }

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
        public void Remove([FromRoute] int id)
        {
            var project = projectRepository.GetById(id);
            if(project != null)
            {
                projectRepository.Remove(project);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        private IEnumerable<ProjectDetailsUserAssigmentViewModel> getProjectDetailsforUser(User user, int projectId)
        {
            //ProjectDetailsViewModel result = new ProjectDetailsViewModel();
            var result = new List<ProjectDetailsUserAssigmentViewModel>();

            var assignmentsByUser = getAssignmentsMadeByUserInProject(user.Id, projectId);

            foreach(var assingmentCollection in assignmentsByUser)
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

        private IDictionary<User,List<Assignment>> getAssignmentsMadeByUserInProject(int userId, int projectId)
        {
            var assignments = queryModel.Assignments.
                             Where(a => a.Assignator.Id == userId && a.Project.Id == projectId)
                             .Include(a => a.HourType)
                             .Include(a => a.Assignee)
                             .ThenInclude(a => a.Role)
                             .GroupBy(a => a.Assignee)
                             .ToDictionary(a => a.Key, a => a.ToList()) ;

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
        public void Create([FromBody]ProjectInputModel projectInputModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Project project = new Project() {
                       ClientId = projectInputModel.Client.Id,
                       TypeId = projectInputModel.ProjectType.Id,
                        Name = projectInputModel.Name
                    };
                    projectRepository.Add(project);
                    Response.StatusCode = (int)HttpStatusCode.Created;
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.Conflict;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        [HttpPut]
        public void Update([FromBody]Project project)
        {
            if (ModelState.IsValid)
            {
                var targetProject = projectRepository.GetById(project.Id);
                if (targetProject != null)
                {
                    targetProject.Name = project.Name;
                    try
                    {
                        projectRepository.Update(targetProject);
                    }
                    catch (Exception e)
                    {
                        Response.StatusCode = (int)HttpStatusCode.Conflict;
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        [HttpGet("{projectId}/getProject")]
        public Project GetProject(int projectId)
        {
            return projectRepository.GetById(projectId);
        }
    }
}
