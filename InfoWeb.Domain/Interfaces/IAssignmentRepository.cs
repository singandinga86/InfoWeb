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
        IEnumerable<Assignment> GetAssignmentsAssignedTo(int userId, int skip = 0, int take = 0);
        IEnumerable<Assignment> GetAssignmentsAssignedBy(int userId, int skip = 0, int take = 0);
        IEnumerable<Assignment> GetAssignments(int userId, int skip = 0, int take = 0);
        Assignment GetAssigmentExist(int idUser, int idHourType, int idProject);
        void removeAssigmentsAssignedTo(int userId, int projectId, int hourTypeId, int assignmentTypeId);
    }
}
