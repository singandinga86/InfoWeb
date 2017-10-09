using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Domain.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        void Remove(User user);
        void Remove(int id);
        void Update(User user);
        IQueryable<User> Users { get; }
    }
}
