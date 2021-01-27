using Library.DataAccess.EF;
using Microsoft.EntityFrameworkCore;

namespace Library.DataAccess.SQLite
{
    public sealed class LibraryWriteContext : WriteContext
    {
        private readonly string dbPath = "Filename=../Library.db";
        public LibraryWriteContext(DbContextOptions<LibraryWriteContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(dbPath,
                sqlOptions =>
                {
                    sqlOptions.CommandTimeout(5);
                });
        }
    }
}
