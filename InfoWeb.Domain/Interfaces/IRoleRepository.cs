using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfoWeb.Domain.Entities;


namespace InfoWeb.Domain.Interfaces
{
    public interface IRoleRepository: IGenericRepository<Role, int>, INamedObjectRepository<Role>, INomenclatorRepository<int>
    {
        IQueryable<Role> Roles { get; }
        IEnumerable<Role> GetSearchRole(string searchValue, int skip = 0, int take = 0);
    }
}
