using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System.Net;

namespace InfoWeb.DistributedServices.Controllers
{
    [Route("api/ProjectTypes")]
    public class ProjectTypesController : Controller
    {
        private IProjectTypeRepository projectTypesRepository;
        public ProjectTypesController(IProjectTypeRepository projectTypesRepository)
        {
            this.projectTypesRepository = projectTypesRepository;
        }

        [HttpGet]
        public IEnumerable<ProjectType> Get()
        {
            return projectTypesRepository.GetAll();
        }

        [HttpGet("{projectTypeId}")]
        public ProjectType GetProjectType([FromRoute] int projectTypeId)
        {
            return projectTypesRepository.GetById(projectTypeId);
        }

        [HttpPost]
        public void Add([FromBody]ProjectType projectType)
        {
            if (projectType != null && projectType.Name.Trim().Length > 0)
            {
                projectType.Projects= null;
                projectTypesRepository.Add(projectType);
                Response.StatusCode = (int)HttpStatusCode.Created;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        [HttpPut]
        public void Update([FromBody] ProjectType projectType)
        {
            if (projectType != null)
            {
                var targetProjectType = projectTypesRepository.GetById(projectType.Id);
                if (targetProjectType != null)
                {
                    targetProjectType.Name = projectType.Name;
                    projectTypesRepository.Update(targetProjectType);
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                }

            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        [HttpDelete("{id}")]
        public void Remove([FromRoute] int id)
        {
            var target = projectTypesRepository.GetById(id);
            if (target != null)
            {
                projectTypesRepository.Remove(target);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
