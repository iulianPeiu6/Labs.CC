using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Youtube.Options;
using Youtube.Services;
using Youtube.Services.Abstracts;

namespace Youtube
{
    public static class YoutubeServiceCollectionExtensions
    {
        public static IServiceCollection AddYoutube(this IServiceCollection services)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("youtubeSettings.json", false)
                .AddJsonFile("youtubeSettings.Development.json", true)
                .AddUserSecrets<YoutubeConfiguration>(true)
                .Build();

            services.Configure<YoutubeConfiguration>(options => configuration.Bind("YoutubeConfig", options));

            services.AddTransient<IYoutubeService, YoutubeService>();

            return services;
        }
    }
}
