using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.Domain.Interfaces
{
    public interface INamedObjectRepository<T> where T: class
    {
        T GetByName(string name);
    }
}
