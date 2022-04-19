using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using UScheduler.Common.SecretsManager;

namespace UScheduler.WebApi.Workspaces
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
