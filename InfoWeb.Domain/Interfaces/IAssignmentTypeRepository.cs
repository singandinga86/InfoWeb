using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InfoWeb.Domain.Interfaces
{
    public interface IAssignmentTypeRepository: IGenericRepository<AssignmentType, int>, 
                                                INamedObjectRepository<AssignmentType>,
                                                INomenclatorRepository<int>
    {
        IQueryable<AssignmentType> AssignmentTypes { get; }
        IEnumerable<AssignmentType> GetSearchAssignmentType(string searchValue, int skip = 0, int take = 0);
    }
}
