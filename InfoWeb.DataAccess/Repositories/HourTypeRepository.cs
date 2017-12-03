using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InfoWeb.DataAccess.Repositories
{
    public class HourTypeRepository : GenericRepository<HourType, int>, IHourTypeRepository
    {
        public HourTypeRepository(InfoWebDatabaseContext context) : base(context)
        {
        }

        public IQueryable<HourType> HourTypes => this.entitySet;

        public bool CanItemBeRemoved(int id)
        {
            return context.ProjectsHoursTypes.Where(pht => pht.HourTypeId == id).FirstOrDefault() == null;
        }

        public IEnumerable<HourType> GetAll()
        {
            return entitySet.ToList();
        }

        public IEnumerable<HourType> GetHourTypeSearch(string searchValue, int skip = 0, int take = 0)
        {
            var query = context.HourTypes
                        .Where(ht => ht.Name.Contains(searchValue) || searchValue == "");

            return base.GetRange(query, skip, take);
        }

        public override HourType GetById(int id)
        {
            return entitySet.Where(h => h.Id == id).FirstOrDefault();
        }

        public HourType GetByName(string name)
        {
            return entitySet.Where(h => h.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<HourType> GetRange(int skip = 0, int take = 0)
        {
            return base.GetRange(entitySet, skip, take);
        }
    }
}
