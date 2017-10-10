using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using System.Linq;

namespace InfoWeb.Domain.Interfaces
{
    public interface IAssignmentRepository: IGenericRepository<Assignment,int>
    {
        IQueryable<Assignment> Assignments { get; }
        IEnumerable<Assignment> GetAssignments(int userId, string assignmentType, int skip = 0, int take = 0);
    }
}
