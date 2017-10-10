using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;

namespace InfoWeb.DataAccess.Repositories
{
    public class InfoWebQueryModel: IInfoWebQueryModel
    {
        private readonly InfoWebDatabaseContext context;
        public InfoWebQueryModel(InfoWebDatabaseContext context)
        {
            this.context = context;
        }

        public IQueryable<User> Users => context.Users;

        public IQueryable<Role> Roles => context.Roles;

        public IQueryable<ProjectType> ProjectType => context.ProjectTypes;

        public IQueryable<ProjectsHoursTypes> ProjectHoursTypes => context.ProjectsHoursTypes;

        public IQueryable<Project> Projects => context.Projects;

        public IQueryable<HourType> HourTypes => context.HourTypes;

        public IQueryable<Client> Clients => context.Clients;

        public IQueryable<AssignmentType> AssignmentTypes => context.AssignmentTypes;

        public IQueryable<Assignment> Assignments => context.Assignments;
    }
}
