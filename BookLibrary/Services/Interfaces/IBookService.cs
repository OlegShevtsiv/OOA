using Services.DTO;
using Services.Filters;

namespace Services.Interfaces
{
    public interface IBookService :IService<BookDTO, IFilter>
    {
    }
}
