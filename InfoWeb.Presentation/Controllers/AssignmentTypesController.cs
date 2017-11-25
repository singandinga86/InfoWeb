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
    [Route("api/AssignmentTypes")]
    public class AssignmentTypesController: Controller
    {

        private IAssignmentTypeRepository assignmentTypeRepository;
        private readonly IUnitOfWork unitOfWork;

        public AssignmentTypesController(IAssignmentTypeRepository assignmentTypeRepository, IUnitOfWork unitOfWork)
        {
            this.assignmentTypeRepository = assignmentTypeRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<AssignmentType> Get()
        {
            return assignmentTypeRepository.GetAll();
        }

        [HttpGet("{assignmentTypeId}")]
        public AssignmentType GetHourType([FromRoute] int assignmentTypeId)
        {
            return assignmentTypeRepository.GetById(assignmentTypeId);
        }

        [HttpPost]
        public IActionResult Add([FromBody]AssignmentType assignmentType)
        {
            if (ModelState.IsValid)
            {
                assignmentType.Assignments = null;
                if (isAssignmentTypeTaken(assignmentType.Name, -1) == false)
                {
                    assignmentTypeRepository.Add(assignmentType);
                    try
                    {
                        unitOfWork.Commit();
                        return Ok("Tipo de asignación <strong>" + assignmentType.Name + "</strong> creado correctamente.");
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new ValidationResult("Error interno del servidor."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("El tipo de asignación <strong>" + assignmentType.Name + "</strong> ya existe."));
                }
            }
           
            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpPut]
        public IActionResult Update([FromBody] AssignmentType assignmentType)
        {
            if (ModelState.IsValid)
            {
                var targetHourType = assignmentTypeRepository.GetById(assignmentType.Id);
                if (targetHourType != null)
                {
                    if (isAssignmentTypeTaken(assignmentType.Name, assignmentType.Id) == false)
                    {
                        targetHourType.Name = assignmentType.Name;
                        assignmentTypeRepository.Update(targetHourType);
                        try
                        {
                            unitOfWork.Commit();
                            return Ok("Tipo de asignación <strong>" + assignmentType.Name + "</strong> actualizado correctamente.");
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("El tipo de asignación <strong>" + assignmentType.Name + "</strong> ya existe."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("Tipo de asignación no encontrado."));
                }
            }
            
            return BadRequest(new ValidationResult("Error en los datos de entrada."));    
        }

        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute] int id)
        {
            var target = assignmentTypeRepository.GetById(id);

            if (target != null)
            {
                if (assignmentTypeRepository.CanItemBeRemoved(target.Id))
                {
                    assignmentTypeRepository.Remove(target);
                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new ValidationResult("Error interno del servidor."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("Este tipo de asignación no puede ser eliminado. Tiene asigaciones asociadas a él."));
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Tipo de asignación no encontrado."));
            }

            return Ok("Tipo de asignación <strong>" + target.Name + "</strong> eliminado correctamente.");
        }

        private bool isAssignmentTypeTaken(string name, int id)
        {
            var target = assignmentTypeRepository.GetByName(name);

            if (target == null || (target != null && id > 0 && target.Id == id))
            {
                return false;
            }

            return true;
        }
    }
}
