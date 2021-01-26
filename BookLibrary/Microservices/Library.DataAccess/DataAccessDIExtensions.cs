using Library.DataAccess.Implementation;
using Library.DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Library.DataAccess
{
    public static class DataAccessDIExtensions
    {
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}