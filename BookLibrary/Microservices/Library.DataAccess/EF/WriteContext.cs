using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.EF
{
    public abstract class WriteContext : CommonContext
    {
        protected WriteContext(DbContextOptions options) : base(options)
        {
        }
    }
}