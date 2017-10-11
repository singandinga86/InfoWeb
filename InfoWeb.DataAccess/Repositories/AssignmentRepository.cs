using InfoWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DataAccess.Repositories
{
    public class AssignmentRepository : GenericRepository<Assignment,int>, IAssignmentRepository
    {
        public AssignmentRepository(InfoWebDatabaseContext context):base(context)
        {
            this.context = context;
        }
        public IQueryable<Assignment> Assignments => context.Assignments;

        public IEnumerable<Assignment> GetRange(int skip, int take)
        {
            var query = context.Assignments.Include(a => a.Project)
                                       .Include(e => e.Assignee)
                                       .Include(e => e.Assignator)
                                       .Include(e => e.HourType)
                                       .Include(e => e.Project);
            return base.GetRange(query, skip, take);
        }

        public IEnumerable<Assignment> GetAll()
        {
            return entitySet.Include(e => e.Assignator)
                   .Include(e => e.Assignee)
                   .Include(e => e.Assignator)
                   .Include(e => e.HourType)
                   .Include(e => e.Project)
                   .ToList();
        }

        public IEnumerable<Assignment> GetAssignments(int userId, int skip = 0, int take = 0)
        {
            var query = context.Assignments.Where(a => a.AssigneeId == userId)
                                        .Include(a => a.Project)
                                        .ThenInclude(a => a.Client)
                                        .Include(a => a.Assignator);


            return base.GetRange(query, skip, take);
        }

        public override Assignment GetById(int id)
        {
            return entitySet.Where(a => a.Id == id).Include(e => e.Assignee)
                   .Include(e => e.Assignee)
                   .Include(e => e.Assignator)
                   .Include(e => e.HourType)
                   .Include(e => e.Project)
                   .FirstOrDefault();
        }       
    }
}
