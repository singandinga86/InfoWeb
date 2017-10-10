using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DataAccess.Repositories
{
    public class ClientRepository : GenericRepository<Client, int>, IClientRepository
    {
        public ClientRepository(InfoWebDatabaseContext context) : base(context)
        {
        }

        public IEnumerable<Client> GetAll()
        {
            return entitySet.ToList();
        }

        public override Client GetById(int id)
        {
            return entitySet.Where(c => c.Id == id).FirstOrDefault();
        }

        public IEnumerable<Client> GetRange(int skip = 0, int take = 0)
        {
            return base.GetRange(entitySet, skip, take);
        }
    }
}
