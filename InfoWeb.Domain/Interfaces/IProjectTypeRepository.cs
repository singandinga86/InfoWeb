﻿using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using System.Linq;

namespace InfoWeb.Domain.Interfaces
{
    public interface IProjectTypeRepository: IGenericRepository<ProjectType, int>,
                                             INamedObjectRepository<ProjectType>,
                                             INomenclatorRepository<int>
    {
        IQueryable<ProjectType> ProjectTypes { get; }
        IEnumerable<ProjectType> GetSearchProjectTypes(string searchValue, int skip = 0, int take = 0);
    }
}
