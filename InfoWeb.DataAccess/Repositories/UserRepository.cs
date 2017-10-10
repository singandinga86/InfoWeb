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

        public IEnumerable<User> GetRange(int skip, int take)
        {
            return base.GetRange(entitySet.Include(u => u.Role), skip, take);
        }
    }
}
