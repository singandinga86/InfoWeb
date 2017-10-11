using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.DistributedServices.Controllers
{
    [Route("api/manager")]
    public class ManageRepositoryController : Controller
    {
        private readonly IAssignmentTypeRepository assigmentTypeRepository;
        private readonly IHourTypeRepository hourTypeRepository;


        public ManageRepositoryController(IAssignmentTypeRepository assigmentTypeRepository, IHourTypeRepository hourTypeRepository)
        {
            this.assigmentTypeRepository = assigmentTypeRepository;
            this.hourTypeRepository = hourTypeRepository;
        }

        [HttpGet("assigmentType")]
        public IEnumerable<AssignmentType> GetAssigmentType()
        {
            IEnumerable<AssignmentType> result = assigmentTypeRepository.GetAll();
            return result;
        }

        [HttpGet("hourType")]
        public IEnumerable<HourType> GetHourType()
        {
            IEnumerable<HourType> result = hourTypeRepository.GetAll();
            return result;
        }
    }
}
