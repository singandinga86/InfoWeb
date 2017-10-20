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


        public AssignmentTypesController(IAssignmentTypeRepository assignmentTypeRepository)
        {
            this.assignmentTypeRepository = assignmentTypeRepository;
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
            if (assignmentType != null && assignmentType.Name.Trim().Length > 0)
            {
                assignmentType.Assignments = null;
                assignmentTypeRepository.Add(assignmentType);
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
            if (assignmentType != null)
            {
                var targetHourType = assignmentTypeRepository.GetById(assignmentType.Id);
                if (targetHourType != null)
                {
                    targetHourType.Name = assignmentType.Name;
                    assignmentTypeRepository.Update(targetHourType);
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
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
