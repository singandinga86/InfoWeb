using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User,int>, IUserRepository
    {

        public UserRepository(InfoWebDatabaseContext context):base(context)
        {
            
        }

        public IQueryable<User> Users { get => context.Users; }

        public IEnumerable<User> GetAll()
        {
            return entitySet.Include(u => u.Role).ToList();
        }

        public override User GetById(int id)
        {
            return entitySet.Where(u => u.Id == id).Include(u => u.Role).FirstOrDefault();
        }

        public User GetByName(string name)
        {
            return context.Users.Where(u => u.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<User> GetRange(int skip, int take)
        {
            return base.GetRange(entitySet.Include(u => u.Role), skip, take);
        }

        public IEnumerable<User> getUsersByRoleName(string roleName, int skip = 0, int take = 0)
        {
            var query = this.entitySet.Where(u => u.Role.Name == roleName);
            return base.GetRange(query, skip, take);
        }

        public IEnumerable<User> getUsersByRoleName(IEnumerable<string> roles, int skip = 0, int take = 0)
        {
            var query = this.entitySet.Where(u => roles.Any(r => r == u.Role.Name)).Include(u => u.Role);
            return base.GetRange(query, skip, take);
        }
    }
}
