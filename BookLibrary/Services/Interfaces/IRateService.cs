using Services.DTO;
using Services.Filters;
namespace Services.Interfaces
{
    public interface IRateService : IService<RateDTO, IFilter>
    {
    }
}
