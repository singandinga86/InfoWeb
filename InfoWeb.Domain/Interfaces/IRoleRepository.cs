using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfoWeb.Domain.Entities;


namespace InfoWeb.Domain.Interfaces
{
    public interface IRoleRepository: IGenericRepository<Role, int>, INamedObjectRepository<Role>
    {
        IQueryable<Role> Roles { get; }
    }
}
