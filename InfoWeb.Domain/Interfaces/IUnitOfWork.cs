using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
