using Services.DTO;
using Services.Filters;

namespace Services.Interfaces
{
    public interface IAuthorService: IService<AuthorDTO, IFilter>
    {
    }
}
