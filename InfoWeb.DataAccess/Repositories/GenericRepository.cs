using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoWeb.DataAccess.Repositories
{
    public abstract class GenericRepository<T, TKey> where T : class
    {
        protected InfoWebDatabaseContext context;
        protected DbSet<T> entitySet;

        public GenericRepository(InfoWebDatabaseContext context)
        {
            this.context = context;
            this.entitySet = context.Set<T>();
        }
        public abstract T GetById(TKey id);
        public virtual void Add(T entity)
        {
            CheckIfEntityIsValid(entity);
            this.entitySet.Add(entity);
            this.context.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            CheckIfEntityIsValid(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
        public virtual void Remove(T entity)
        {
            CheckIfEntityIsValid(entity);
            this.entitySet.Remove(entity);
            this.context.SaveChanges();
        }
        protected IEnumerable<T> GetRange(IQueryable<T> sourceQuery, int skip = 0, int take = 0)
        {
           /* if(skip < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }
            
            var query = sourceQuery.Skip(skip);
            if(take < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(take));
            }

            if(take > 0)
            {
                query = query.Take(take);
            }*/
            return sourceQuery.ToList();
        }

        protected void CheckIfEntityIsValid(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
        }
    }
}
