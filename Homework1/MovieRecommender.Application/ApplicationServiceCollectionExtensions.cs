using Microsoft.Extensions.DependencyInjection;
using MovieRecommender.Application.Interfaces;
using MovieRecommender.Application.Services;

namespace MovieRecommender.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IMovieRecommendationService, MovieRecommendationService>();
            services.AddTransient<IAPIsReportingService, APIsReportingService>();

            return services;
        }
    }
}
