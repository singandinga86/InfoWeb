using System;
using System.Collections.Generic;
using System.Text;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Domain.Interfaces
{
    public interface IClientRepository: IGenericRepository<Client, int>
    {
    }
}
