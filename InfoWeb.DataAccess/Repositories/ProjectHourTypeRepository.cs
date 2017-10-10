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

        public IEnumerable<ProjectsHoursTypes> GetRange(int skip = 0, int take = 0)
        {
            return base.GetRange(entitySet
                   .Include(p => p.Project)
                   .Include(p => p.HourType), skip, take);
        }
    }
}
