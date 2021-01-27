using System;
using System.Linq;
using System.Linq.Expressions;
using Library.DataAccess.EF;
using Library.DataAccess.Interfaces;
using Library.DataAccess.SQLite;

namespace Library.DataAccess.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly ReadContext Context;

        public Repository(ReadContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> Get()
        {
            return Context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }
    }
}
