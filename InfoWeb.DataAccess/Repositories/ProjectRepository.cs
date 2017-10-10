using InfoWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DataAccess.Repositories
{
    public class ProjectRepository : GenericRepository<Project,int>, IProjectRepository
    {
        public ProjectRepository(InfoWebDatabaseContext context): base(context)
        {
         
        }

        public IQueryable<Project> Projects => context.Projects;

        public IEnumerable<Project> GetAll()
        {
            return entitySet.Include(p => p.Client)
                  .Include(p => p.ProjectsHoursTypes)
                  .Include(p => p.Type)
                  .ToList();
        }

        public override Project GetById(int id)
        {
            return entitySet.Where(p => p.Id == id)
                  .Include(p => p.Client)
                  .Include(p => p.ProjectsHoursTypes)
                  .Include(p => p.Type)
                  .FirstOrDefault();
        }

        public IEnumerable<Project> GetRange(int skip, int take)
        {
            return base.GetRange(entitySet
                  .Include(p => p.Client)
                  .Include(p => p.ProjectsHoursTypes)
                  .Include(p => p.Type), skip, take);
        }
    }
}
