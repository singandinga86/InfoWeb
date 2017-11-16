using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Domain.Interfaces
{
    public interface IUserRepository: IGenericRepository<User,int>, INamedObjectRepository<User>
    {
        IQueryable<User> Users { get; }
        IEnumerable<User> getUsersByRoleName(string roleName, int skip = 0, int take = 0);
        IEnumerable<User> getUsersByRoleName(IEnumerable<string> roles, int skip = 0, int take = 0);
    }
}
