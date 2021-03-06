﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Domain.Interfaces
{
    public interface IHourTypeRepository: IGenericRepository<HourType,int>, 
                                          INamedObjectRepository<HourType>,
                                          INomenclatorRepository<int>
    {
        IQueryable<HourType> HourTypes { get; }
        IEnumerable<HourType> GetHourTypeSearch(string searchValue, int skip = 0, int take = 0);
    }
}
