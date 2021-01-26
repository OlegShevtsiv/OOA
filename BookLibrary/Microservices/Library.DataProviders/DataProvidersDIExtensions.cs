using Library.DataAccess;
using Library.DataProviders.Implementation;
using Library.DataProviders.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Library.DataProviders
{
    public static class DataProvidersDIExtensions
    {
        public static void AddDataProviders(this IServiceCollection services)
        {
            services.AddUnitOfWork();
            services.AddScoped<IRateProvider, RateProvider>();
            services.AddScoped<IBookProvider, BookProvider>();
            services.AddScoped<IAuthorProvider, AuthorProvider>();
            services.AddScoped<ICommentProvider, CommentProvider>();
        }
    }
}