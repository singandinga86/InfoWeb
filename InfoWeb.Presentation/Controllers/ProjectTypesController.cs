using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System.Net;
using InfoWeb.Presentation.Models;

namespace InfoWeb.Presentation.Controllers
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
        public IActionResult Add([FromBody]ProjectType projectType)
        {
            if (ModelState.IsValid)
            {
                projectType.Projects= null;
                if (isProjectTypeTaken(projectType.Name, -1) == false)
                {
                    projectTypesRepository.Add(projectType);
                    try
                    {
                        unitOfWork.Commit();
                        return Ok();
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new ValidationResult("Error interno del servidor."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("El tipo de proyecto ya existe."));
                }
            }
            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpPut]
        public IActionResult Update([FromBody] ProjectType projectType)
        {
            if (ModelState.IsValid)
            {
                var targetProjectType = projectTypesRepository.GetById(projectType.Id);
                if (targetProjectType != null)
                {
                    if (isProjectTypeTaken(projectType.Name, projectType.Id) == false)
                    {
                        targetProjectType.Name = projectType.Name;
                        projectTypesRepository.Update(targetProjectType);
                        try
                        {
                            unitOfWork.Commit();
                            return Ok();
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("El tipo de proyecto ya existe."));
                    }                    
                }
                else
                {
                    return BadRequest(new ValidationResult("Tipo de proyecto no encontrado."));
                }

            }
            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute] int id)
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
                    return BadRequest(new ValidationResult("Error interno del servidor."));
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok();
        }

        private bool isProjectTypeTaken(string userName, int id)
        {
            var user = this.projectTypesRepository.GetByName(userName);

            if (user == null || (user != null && id > 0 && user.Id == id))
            {
                return false;
            }

            return true;
        }
    }
}
