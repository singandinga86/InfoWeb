﻿using InfoWeb.Domain.Entities;
using InfoWeb.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoWeb.DataAccess.Repositories
{
    public class AssignmentTypeRepository : GenericRepository<AssignmentType, int>, IAssignmentTypeRepository
    {
        public AssignmentTypeRepository(InfoWebDatabaseContext context) : base(context)
        {
        }

        public IQueryable<AssignmentType> AssignmentTypes => this.entitySet;

        public bool CanItemBeRemoved(int id)
        {
            return context.Assignments.Where(a => a.AssignmentTypeId == id).FirstOrDefault() == null;
        }

        public IEnumerable<AssignmentType> GetAll()
        {
            return entitySet.ToList();
        }

        public IEnumerable<AssignmentType> GetSearchAssignmentType(string searchValue,int skip = 0, int take = 0)
        {
            var query = context.AssignmentTypes
                         .Where(at => at.Name.Contains(searchValue) || searchValue == "");

            return base.GetRange(query, skip, take);

        }

        public override AssignmentType GetById(int id)
        {
            return entitySet.Where(a => a.Id == id).FirstOrDefault();
        }

        public AssignmentType GetByName(string name)
        {
            return entitySet.Where(at => at.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public IEnumerable<AssignmentType> GetRange(int skip = 0, int take = 0)
        {
            return base.GetRange(entitySet, skip, take);
        }
    }
}
