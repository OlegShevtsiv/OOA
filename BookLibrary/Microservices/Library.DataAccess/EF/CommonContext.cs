using Library.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.EF
{
    public abstract class CommonContext  : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rate> Rates { get; set; }

        protected CommonContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}