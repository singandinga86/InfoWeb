using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InfoWeb.Domain.Interfaces
{
    public interface IAssignmentTypeRepository: IGenericRepository<AssignmentType, int>
    {
        IQueryable<AssignmentType> AssignmentTypes { get; }
    }
}
