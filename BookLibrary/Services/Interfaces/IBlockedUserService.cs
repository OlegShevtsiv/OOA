using Services.DTO;
using Services.Filters;

namespace Services.Interfaces
{
    public interface IBlockedUserService : IService<BlockedUserDTO, IFilter>
    {
    }
}
