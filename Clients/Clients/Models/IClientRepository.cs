using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clients.Models
{
    public interface IClientRepository
    {
        IEnumerable<Clientes> GetClients(string searchCriteria);
        Clientes GetClient(long id);
        int Create(Clientes client);
        int Update(Clientes client);
        int Delete(long id);
    }
}
