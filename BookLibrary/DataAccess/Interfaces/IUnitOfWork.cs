using System;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        int SaveChanges();
    }
}
