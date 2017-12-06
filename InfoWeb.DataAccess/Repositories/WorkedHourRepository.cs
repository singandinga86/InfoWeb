using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoWeb.DataAccess.Repositories
{
    public class WorkedHourRepository: GenericRepository<WorkedHour, int>, IWorkedHourRepository
    {
        public WorkedHourRepository(InfoWebDatabaseContext context):
               base(context)
        {

        }
        public override WorkedHour GetById(int id)
        {
            return entitySet.Where(wo => wo.Id == id).FirstOrDefault();
        }

        public IEnumerable<WorkedHour> GetRange(int skip = 0, int take = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WorkedHour> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<WorkedHour> WorkedHours => context.WorkedHours;

    }
}
