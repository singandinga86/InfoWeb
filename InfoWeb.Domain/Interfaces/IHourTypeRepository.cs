using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Domain.Interfaces
{
    public interface IHourTypeRepository: IGenericRepository<HourType,int>
    {
        IQueryable<HourType> HourTypes { get; }
    }
}
