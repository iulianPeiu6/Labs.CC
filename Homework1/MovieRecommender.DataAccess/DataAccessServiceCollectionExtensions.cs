using Microsoft.Extensions.DependencyInjection;
using MovieRecommender.Application.Interfaces;

namespace MovieRecommender.DataAccess
{
    public static class DataAccessServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddTransient<IRequestLogRepository, RequestLogRepository>();

            return services;
        }
    }
}
