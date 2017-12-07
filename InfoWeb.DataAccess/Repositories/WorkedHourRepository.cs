using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<WorkedHour> GetworkedHoursByUser(int userId, string search)
        {
            string searchValue = search == null? "":search.Trim().ToLower();
        
            var result =  context.WorkedHours  
                
                   .Where(wh => wh.UserId == userId && (searchValue == "" || wh.Description.ToLower().Contains(searchValue) || wh.Hours.ToString().Contains(searchValue) ||
                   wh.ProjectHourType.HourType.Name.ToLower().Contains(searchValue) || wh.ProjectHourType.Project.Name.ToLower().Contains(searchValue)))
                   .Include(wh => wh.ProjectHourType)
                   .ThenInclude(wh => wh.Project)
                   .Include(wh => wh.ProjectHourType.HourType)
                   .Include(wh => wh.User).ToList();
            return result.ToList();
        }

    }
}
