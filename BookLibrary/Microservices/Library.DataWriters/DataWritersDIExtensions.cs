using Library.DataWriters.Implementations;
using Library.DataWriters.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Library.DataWriters
{
    public static class DataWritersDIExtensions
    {
        public static void AddDataWriters(this IServiceCollection services)
        {
            services.AddScoped<IRateDataWriter, RateDataWriter>();
            services.AddScoped<IBookDataWriter, BookDataWriter>();
            services.AddScoped<IAuthorDataWriter, AuthorDataWriter>();
            services.AddScoped<ICommentDataWriter, CommentDataWriter>();
        }
    }
}