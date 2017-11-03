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
        private readonly IUnitOfWork unitOfWork;

        public ProjectTypesController(IProjectTypeRepository projectTypesRepository, IUnitOfWork unitOfWork)
        {
            this.projectTypesRepository = projectTypesRepository;
            this.unitOfWork = unitOfWork;
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
            if (ModelState.IsValid)
            {
                projectType.Projects= null;
                projectTypesRepository.Add(projectType);
                try
                {
                    unitOfWork.Commit();
                    Response.StatusCode = (int)HttpStatusCode.Created;
                }
                catch(Exception e)
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
        public void Update([FromBody] ProjectType projectType)
        {
            if (ModelState.IsValid)
            {
                var targetProjectType = projectTypesRepository.GetById(projectType.Id);
                if (targetProjectType != null)
                {
                    targetProjectType.Name = projectType.Name;
                    projectTypesRepository.Update(targetProjectType);
                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch(Exception e)
                    {
                        Response.StatusCode = (int)HttpStatusCode.Conflict;
                    }
                    
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
                try {
                    unitOfWork.Commit();
                }
                catch(Exception e)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
