using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TMDb.Options;
using TMDb.Services;
using TMDb.Services.Abstracts;

namespace TMDb
{
    public static class TMDbServiceCollectionExtensions
    {
        public static IServiceCollection AddTMDb(this IServiceCollection services)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("tmdbSettings.json", false)
                .AddJsonFile("tmdbSettings.Development.json", true)
                .AddUserSecrets<TMDbConfiguration>(true)
                .Build();

            services.Configure<TMDbConfiguration>(options => configuration.Bind("TMDbConfig", options));

            services.AddTransient<ITMDbService, TMDbService>();

            return services;
        }
    }
}
