using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IService<TDto, TFilter>
    {
        TDto Get(string id);
        IEnumerable<TDto> Get(TFilter filter);
        IEnumerable<TDto> GetAll();
        void Add(TDto dto);
        void Remove(string id);
        void Update(TDto dto);
    }
}
