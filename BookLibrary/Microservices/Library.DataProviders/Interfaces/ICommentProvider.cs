using Library.DataAccess.DTO;
using Library.DataProviders.Filters;
using Services.Interfaces;

namespace Library.DataProviders.Interfaces
{
    public interface ICommentProvider : IProvider<CommentDTO, IFilter>
    {
    }
}
