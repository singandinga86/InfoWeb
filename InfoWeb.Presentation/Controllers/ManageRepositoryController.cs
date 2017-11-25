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
        private readonly IUserRepository userRepository;
        private readonly IAssignmentRepository assignmentRepository;




        public ManageRepositoryController(IAssignmentTypeRepository assigmentTypeRepository,
                                          IHourTypeRepository hourTypeRepository,
                                          IProjectHourTypeRepository projectHourTypeRepository,
                                          IUserRepository userRepository,
                                          IAssignmentRepository assignmentRepository)
        {
            this.assigmentTypeRepository = assigmentTypeRepository;
            this.hourTypeRepository = hourTypeRepository;
            this.projectHourTypeRepository = projectHourTypeRepository;
            this.userRepository = userRepository;
            this.assignmentRepository = assignmentRepository;
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

        [HttpGet("hourTypeByProject/{idProject}/{userId}")]
        public IEnumerable<HourType> GetHourTypeByProject([FromRoute]int idProject, [FromRoute]int userId)
        {
            User user = userRepository.GetById(userId);
            List<HourType> hoursType = new List<HourType>();

            if (user.Role.Name == "TAM" || user.Role.Name == "PM")
            {
                var assignments = assignmentRepository.Assignments
                                  .Where(a => a.ProjectId == idProject && a.AssigneeId == userId && a.AssignmentType.Name == "Delegar proyecto" 
                                  && a.HourTypeId != null)
                                  .ToList();

                if(assignments.Count > 0)
                {
                    assignments.ForEach(a =>
                    {
                        hoursType.Add(hourTypeRepository.GetById(a.HourTypeId.Value));
                    });
                }
                else
                {
                    IEnumerable<ProjectsHoursTypes> result = projectHourTypeRepository.GetHourTypeByProject(idProject);

                    foreach (var projectHourType in result)
                    {
                        hoursType.Add(hourTypeRepository.GetById(projectHourType.HourTypeId));
                    }
                }
              
            }
            else
            {
                IEnumerable<ProjectsHoursTypes> result = projectHourTypeRepository.GetHourTypeByProject(idProject);
               
                foreach (var projectHourType in result)
                {
                    hoursType.Add(hourTypeRepository.GetById(projectHourType.HourTypeId));
                }
            }

           
            return hoursType;
        }
    }
}
