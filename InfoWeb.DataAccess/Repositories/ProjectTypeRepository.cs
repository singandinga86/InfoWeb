using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.DataAccess.Repositories
{
    public class ProjectTypeRepository : GenericRepository<ProjectType, int>, IProjectTypeRepository
    {
        public ProjectTypeRepository(InfoWebDatabaseContext context) : base(context)
        {
        }

        public IQueryable<ProjectType> ProjectTypes => this.entitySet;

        public bool CanItemBeRemoved(int id)
        {
            return context.Projects.Where(p => p.TypeId == id).FirstOrDefault() == null;
        }

        public IEnumerable<ProjectType> GetAll()
        {
            return entitySet.ToList();
        }

        public IEnumerable<ProjectType> GetSearchProjectTypes(string searchValue, int skip = 0, int take = 0)
        {
            var query = context.ProjectTypes
                         .Where(pt => pt.Name.Contains(searchValue) || searchValue == "");

            return base.GetRange(query, skip, take);
        }

        public override ProjectType GetById(int id)
        {
            return entitySet.Where(pt => pt.Id == id).FirstOrDefault();
        }

        public ProjectType GetByName(string name)
        {
            return entitySet.Where(pt => pt.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<ProjectType> GetRange(int skip, int take)
        {
            return base.GetRange(entitySet, skip, take);
        }
    }
}
