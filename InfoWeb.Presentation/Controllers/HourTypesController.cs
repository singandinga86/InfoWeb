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
    [Route("api/HourTypes")]
    public class HourTypesController : Controller
    {
        private IHourTypeRepository hourTypeRepository;
        private readonly IUnitOfWork unitOfWork;
        public HourTypesController(IHourTypeRepository hourTypeRepository, IUnitOfWork unitOfWork)
        {
            this.hourTypeRepository = hourTypeRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<HourType> Get()
        {
            return hourTypeRepository.GetAll();
        }

        [HttpGet("search/{searchValue?}")]
        public IEnumerable<HourType> getSearchHourType([FromRoute]string searchValue = "")
        {
            return hourTypeRepository.GetHourTypeSearch(searchValue);
        }

        [HttpGet("{hourTypeId}")]
        public HourType GetHourType([FromRoute] int hourTypeId)
        {
            return hourTypeRepository.GetById(hourTypeId);
        }

        [HttpPost]
        public IActionResult Add([FromBody]HourType hourType)
        {
            if(ModelState.IsValid)
            {
                if (isHourTypeTaken(hourType.Name, -1) == false)
                {
                    hourType.ProjectsHoursTypes = null;
                    hourTypeRepository.Add(hourType);
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
                    return BadRequest(new ValidationResult("El tipo de hora <strong>" + hourType.Name + "</strong> ya existe."));
                }
            }
           
            return BadRequest(new ValidationResult("Error en los datos de entrada."));
        }

        [HttpPut]
        public IActionResult Update([FromBody] HourType hourType)
        {
            if (ModelState.IsValid)
            {
                var targetHourType = hourTypeRepository.GetById(hourType.Id);
                if (targetHourType != null)
                {
                    if (isHourTypeTaken(hourType.Name, hourType.Id) == false)
                    {
                        targetHourType.Name = hourType.Name;
                        hourTypeRepository.Update(targetHourType);
                        try
                        {
                            unitOfWork.Commit();
                            return Ok("Tipo de hora <strong>" + targetHourType.Name + "</strong> creado correctamente.");
                        }
                        catch (Exception e)
                        {
                            return BadRequest(new ValidationResult("Error interno del servidor."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ValidationResult("El tipo de hora <strong>" + targetHourType.Name +"</strong> ya existe."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("Tipo de hora no encontrado."));
                }

            }

            return BadRequest(new ValidationResult("Error en los datos de entrada."));

            
        }

        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute] int id)
        {
            var target = hourTypeRepository.GetById(id);
            if(target != null)
            {
                if (hourTypeRepository.CanItemBeRemoved(target.Id))
                {
                    hourTypeRepository.Remove(target);
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
                    return BadRequest(new ValidationResult("El tipo de hora <strong>" + target.Name +"</strong> no se puede eliminar. Tiene proyectos asociados a él."));
                }
            }
            else
            {
                return BadRequest(new ValidationResult("Tipo de hora no encontrado."));
            }

            return Ok("Tipo de hora <strong>" + target.Name + "</strong> actualizado correctamente.");
        }

        private bool isHourTypeTaken(string name, int id)
        {
            var target = hourTypeRepository.GetByName(name);

            if (target == null || (target != null && id > 0 && target.Id == id))
            {
                return false;
            }

            return true;
        }
    }
}
