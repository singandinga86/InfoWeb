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
        public void Add([FromBody]AssignmentType assignmentType)
        {
            if (ModelState.IsValid)
            {
                assignmentType.Assignments = null;
                assignmentTypeRepository.Add(assignmentType);
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
        public void Update([FromBody] AssignmentType assignmentType)
        {
            if (ModelState.IsValid)
            {
                var targetHourType = assignmentTypeRepository.GetById(assignmentType.Id);
                if (targetHourType != null)
                {
                    targetHourType.Name = assignmentType.Name;
                    assignmentTypeRepository.Update(targetHourType);
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
            var target = assignmentTypeRepository.GetById(id);

            if (target != null)
            {
                assignmentTypeRepository.Remove(target);
                try
                {
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
