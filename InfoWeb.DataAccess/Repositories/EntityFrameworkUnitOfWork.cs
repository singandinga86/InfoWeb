using InfoWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.DataAccess.Repositories
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly InfoWebDatabaseContext context;
        public EntityFrameworkUnitOfWork(InfoWebDatabaseContext context)
        {
            this.context = context;
        }
        public void Commit()
        {
            context.SaveChanges();
        }
    }
}
