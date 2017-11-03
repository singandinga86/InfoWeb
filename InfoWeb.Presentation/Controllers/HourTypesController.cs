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
        public void Add([FromBody]HourType hourType)
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
                    Response.StatusCode = (int)HttpStatusCode.Conflict;
                }
                
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
            var target = hourTypeRepository.GetById(id);
            if(target != null)
            {
                hourTypeRepository.Remove(target);
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
