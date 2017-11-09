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
                hourType.ProjectsHoursTypes = null;
                hourTypeRepository.Add(hourType);
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

        [HttpPut]
        public IActionResult Update([FromBody] HourType hourType)
        {
            if (ModelState.IsValid)
            {
                var targetHourType = hourTypeRepository.GetById(hourType.Id);
                if (targetHourType != null)
                {
                    targetHourType.Name = hourType.Name;
                    hourTypeRepository.Update(targetHourType);
                    try
                    {
                        unitOfWork.Commit();
                    }
                    catch(Exception e)
                    {
                        return BadRequest(new ValidationResult("Error interno del servidor."));
                    }
                }
                else
                {
                    return BadRequest(new ValidationResult("Tipo de hora no encontrado."));
                }

            }
            else
            {
                return BadRequest(new ValidationResult("Error en los datos de entrada."));
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute] int id)
        {
            var target = hourTypeRepository.GetById(id);
            if(target != null)
            {
                hourTypeRepository.Remove(target);
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
                return BadRequest(new ValidationResult("Tipo de hora no encontrado."));
            }

            return Ok();
        }
    }
}
