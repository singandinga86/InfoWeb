using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;

using System.Linq;
using InfoWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DataAccess.Repositories
{
    public class ProjectHourTypeRepository : GenericRepository<ProjectsHoursTypes, int>, IProjectHourTypeRepository
    {
        public ProjectHourTypeRepository(InfoWebDatabaseContext context) : base(context)
        {
        }

        public IQueryable<ProjectsHoursTypes> ProjectHourTypes => this.entitySet;

        public IEnumerable<ProjectsHoursTypes> GetAll()
        {
            return entitySet.ToList();
        }

        public override ProjectsHoursTypes GetById(int id)
        {
            return entitySet.Where(pht => pht.Id == id)
                   .Include(p => p.Project)
                   .Include(p => p.HourType)
                   .FirstOrDefault();
        }

        public IEnumerable<ProjectsHoursTypes> GetListById(int id)
        {
            return entitySet.Where(pht => pht.ProjectId == id)
                   .Include(p => p.Project)
                   .Include(p => p.HourType)
                   .ToList();
        }

        public IEnumerable<ProjectsHoursTypes> GetRange(int skip = 0, int take = 0)
        {
            return base.GetRange(entitySet
                   .Include(p => p.Project)
                   .Include(p => p.HourType), skip, take);
        }

        public ProjectsHoursTypes GetHourType(int idProject, int idHourType)
        {
            return context.ProjectsHoursTypes.Where(pht => pht.HourTypeId == idHourType && pht.ProjectId == idProject).FirstOrDefault();
        }

        public IEnumerable<ProjectsHoursTypes> GetHourTypeByProject(int idProject)
        {
            return context.ProjectsHoursTypes.Where(pht => pht.ProjectId == idProject).ToList();
        }
    }
}
