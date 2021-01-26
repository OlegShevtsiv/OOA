using Library.DataAccess.EF;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.SQLite
{
    public sealed class LibraryReadContext : ReadContext
    {
        private readonly string dbPath = "Filename=../Library.db";
        public LibraryReadContext(DbContextOptions<LibraryReadContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(dbPath);
        }
    }
}
