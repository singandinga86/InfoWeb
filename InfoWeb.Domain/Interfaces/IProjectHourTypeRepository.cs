using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Domain.Interfaces
{
    public interface IProjectHourTypeRepository: IGenericRepository<ProjectsHoursTypes, int>
    {
        IQueryable<ProjectsHoursTypes> ProjectHourTypes { get; }
        IEnumerable<ProjectsHoursTypes> GetListById(int id);
        ProjectsHoursTypes GetHourType(int idProject, int idHourType);
        IEnumerable<ProjectsHoursTypes> GetHourTypeByProject(int idProject);
        bool CanItemBeRemoved(int id, int projectId);
    }
}
