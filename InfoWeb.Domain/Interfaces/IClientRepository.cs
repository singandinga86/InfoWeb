using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using InfoWeb.Domain.Entities;

namespace InfoWeb.Domain.Interfaces
{
    public interface IClientRepository: IGenericRepository<Client, int>,
                                        INamedObjectRepository<Client>, 
                                        INomenclatorRepository<int>
    {
        IQueryable<Client> Clients { get; }
    }
}
