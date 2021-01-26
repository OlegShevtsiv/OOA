using Library.DataAccess.DTO;
using Library.DataProviders.Filters;
using Services.Interfaces;

namespace Library.DataProviders.Interfaces
{
    public interface IRateProvider : IProvider<RateDTO, IFilter>
    {
    }
}
