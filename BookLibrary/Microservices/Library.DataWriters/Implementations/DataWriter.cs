using System.Collections.Generic;
using Library.DataAccess.EF;
using Library.DataWriters.Interfaces;

namespace Library.DataWriters.Implementations
{
    internal abstract class DataWriter<TEntity, TDto> : IDataWriter<TDto>
    where TEntity : class
    where TDto : class
    {
        protected readonly WriteContext Context;

        protected DataWriter(WriteContext unitOfWork)
        {
            Context = unitOfWork;
        }

        protected abstract TDto MapToDto(TEntity entity);
        protected abstract TEntity MapToEntity(TDto dto);
        
        public abstract void Add(TDto dto);
        public abstract void Remove(string id);
        public abstract void Update(TDto dto);
    }
}
