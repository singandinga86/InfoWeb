using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;
using System.Linq;

namespace InfoWeb.Domain.Interfaces
{
    public interface IProjectTypeRepository: IGenericRepository<ProjectType, int>
    {
    }
}
