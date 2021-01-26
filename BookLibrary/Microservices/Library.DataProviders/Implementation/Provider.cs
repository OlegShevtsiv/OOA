using System.Collections.Generic;
using Library.DataAccess.Interfaces;
using Services.Interfaces;

namespace Library.DataProviders.Implementation
{
    public abstract class Provider<TEntity, TDto, TFilter> : IProvider<TDto, TFilter>
    where TEntity : class
    where TDto : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected Provider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected abstract TDto MapToDto(TEntity entity);
        protected abstract TEntity MapToEntity(TDto dto);

        protected IRepository<TEntity> Repository => _unitOfWork.GetRepository<TEntity>();

        public abstract TDto Get(string id);
        public abstract IEnumerable<TDto> Get(TFilter filter);
        public abstract IEnumerable<TDto> GetAll();
        // public abstract void Add(TDto dto);
        // public abstract void Remove(string id);
        // public abstract void Update(TDto dto);
    }
}
