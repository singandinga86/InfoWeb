using InfoWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using System.Linq;

namespace InfoWeb.DataAccess.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly InfoWebDatabaseContext context;
        public AssignmentRepository(InfoWebDatabaseContext context)
        {
            this.context = context;
        }
        public IQueryable<Assignment> Assignments => context.Assignments;

        
        public IEnumerable<Project> GetAssignedProjects(int userId, string assignmentType)
        {
            /*var q = from a in context.Assignments join p in context.Projects on a.ProjectId equals p.Id into g
                     join c in context.Clients on g.*/

            return null;


            
        }
    }
}
