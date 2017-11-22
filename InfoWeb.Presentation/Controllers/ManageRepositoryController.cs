using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoWeb.Presentation.Controllers
{
    [Route("api/manager")]
    public class ManageRepositoryController : Controller
    {
        private readonly IAssignmentTypeRepository assigmentTypeRepository;
        private readonly IHourTypeRepository hourTypeRepository;
        private readonly IProjectHourTypeRepository projectHourTypeRepository;



        public ManageRepositoryController(IAssignmentTypeRepository assigmentTypeRepository,
                                          IHourTypeRepository hourTypeRepository,
                                          IProjectHourTypeRepository projectHourTypeRepository)
        {
            this.assigmentTypeRepository = assigmentTypeRepository;
            this.hourTypeRepository = hourTypeRepository;
            this.projectHourTypeRepository = projectHourTypeRepository;
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

        [HttpGet("hourTypeByProject/{idProject}")]
        public IEnumerable<HourType> GetHourTypeByProject([FromRoute]int idProject)
        {
            IEnumerable<ProjectsHoursTypes> result = projectHourTypeRepository.GetHourTypeByProject(idProject);
            List<HourType> hoursType = new List<HourType>();
            foreach(var projectHourType in result)
            {
                hoursType.Add(hourTypeRepository.GetById(projectHourType.HourTypeId));
            }
            return hoursType;
        }
    }
}
