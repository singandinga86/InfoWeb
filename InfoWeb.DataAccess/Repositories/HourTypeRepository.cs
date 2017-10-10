﻿using System;
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

        public IEnumerable<HourType> GetAll()
        {
            return entitySet.ToList();
        }

        public override HourType GetById(int id)
        {
            return entitySet.Where(h => h.Id == id).FirstOrDefault();
        }

        public IEnumerable<HourType> GetRange(int skip = 0, int take = 0)
        {
            return base.GetRange(entitySet, skip, take);
        }
    }
}
