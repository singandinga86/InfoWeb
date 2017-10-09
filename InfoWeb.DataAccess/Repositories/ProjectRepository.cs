using InfoWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using System.Linq;

namespace InfoWeb.DataAccess.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly InfoWebDatabaseContext context;

        public ProjectRepository(InfoWebDatabaseContext context)
        {
            this.context = context;
        }
        public IQueryable<Project> Projects => context.Projects;
    }
}
