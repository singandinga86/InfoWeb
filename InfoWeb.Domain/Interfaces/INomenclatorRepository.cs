using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.Domain.Interfaces
{
    public interface INomenclatorRepository<TKey>
    {
        bool CanItemBeRemoved(TKey id);
    }
}
