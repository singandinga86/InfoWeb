using System;
using System.Collections.Generic;
using System.Text;

namespace InfoWeb.Domain.Interfaces
{
    public interface IGenericRepository<T, TKey>
    {
        T GetById(TKey id);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        IEnumerable<T> GetRange(int skip = 0, int take = 0);
        IEnumerable<T> GetAll();
    }
}
