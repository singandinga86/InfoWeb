using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoWeb.DataAccess.Repositories
{
    public class WorkedHourRepository: GenericRepository<WorkedHour, int>
    {
        public WorkedHourRepository(InfoWebDatabaseContext context):
               base(context)
        {

        }
        public override WorkedHour GetById(int id)
        {
            return entitySet.Where(wo => wo.Id == id).FirstOrDefault();
        }

        public IQueryable<WorkedHour> WorkedHours => context.WorkedHours;

    }
}
