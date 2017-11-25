using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;

namespace InfoWeb.DataAccess.Repositories
{
    public class RoleRepository : GenericRepository<Role, int>, IRoleRepository
    {
        public RoleRepository(InfoWebDatabaseContext context) : base(context)
        {
        }

        public IQueryable<Role> Roles => entitySet;

        public bool CanItemBeRemoved(int id)
        {
            return context.Users.Where(u => u.Role.Id == id).FirstOrDefault() == null;
        }

        public IEnumerable<Role> GetAll()
        {
            return entitySet.ToList();
        }

        public override Role GetById(int id)
        {
            return entitySet.Where(r => r.Id == id).FirstOrDefault();
        }

        public Role GetByName(string name)
        {
            return entitySet.Where(r => r.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<Role> GetRange(int skip, int take)
        {
            return base.GetRange(entitySet, skip, take);
        }
    }
}
