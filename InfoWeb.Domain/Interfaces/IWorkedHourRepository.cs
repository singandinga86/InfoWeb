using InfoWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoWeb.Domain.Interfaces
{
    public interface IWorkedHourRepository: IGenericRepository<WorkedHour, int>
    {
        IQueryable<WorkedHour> WorkedHours { get; }
    }
}
