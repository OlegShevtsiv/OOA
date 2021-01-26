using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.EF
{
    public abstract class ReadContext : CommonContext
    {
        protected ReadContext(DbContextOptions options) : base(options)
        {
        }
    }
}