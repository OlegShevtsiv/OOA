using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void Remove(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
    }
}
