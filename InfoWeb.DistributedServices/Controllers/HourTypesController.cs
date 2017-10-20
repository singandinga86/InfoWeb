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
    [Route("api/HourTypes")]
    public class HourTypesController : Controller
    {
        private IHourTypeRepository hourTypeRepository;
        public HourTypesController(IHourTypeRepository hourTypeRepository)
        {
            this.hourTypeRepository = hourTypeRepository;
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
        public void Add([FromBody]HourType hourType)
        {
            if (hourType != null && hourType.Name.Trim().Length > 0)
            {
                hourType.ProjectsHoursTypes = null;
                hourTypeRepository.Add(hourType);
                Response.StatusCode = (int)HttpStatusCode.Created;
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }

        [HttpPut]
        public void Update([FromBody] HourType hourType)
        {
            if (hourType != null)
            {
                var targetHourType = hourTypeRepository.GetById(hourType.Id);
                if (targetHourType != null)
                {
                    targetHourType.Name = hourType.Name;
                    hourTypeRepository.Update(targetHourType);
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
            var target = hourTypeRepository.GetById(id);
            if(target != null)
            {
                hourTypeRepository.Remove(target);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
