using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;

namespace InfoWeb.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InfoWebDatabaseContext context;

        public UserRepository(InfoWebDatabaseContext context)
        {
            this.context = context;
        }

        public IQueryable<User> Users { get => context.Users; }

        public void Add(User user)
        {
            
        }

        public void Remove(User user)
        {
            
        }

        public void Remove(int id)
        {
            
        }

        public void Update(User user)
        {
            
        }
    }
}
