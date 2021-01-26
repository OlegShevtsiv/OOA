using System;
using System.Collections.Generic;
using Library.DataAccess.Interfaces;
using Library.DataAccess.SQLite;

namespace Library.DataAccess.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryReadContext _context;

        private Dictionary<Type, object> _repositories;

        public UnitOfWork(LibraryReadContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return  _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            Type type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(_context);
            }

            return (IRepository<TEntity>)_repositories[type];
        }
    }
}
