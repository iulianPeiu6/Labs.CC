using Microsoft.Extensions.Configuration;

namespace UScheduler.Common.ConfigurationBinder
{
    public static class ConfigurationBinder
    {
        public static TSettings GetConfiguration<TSettings>(string configurationFileName) where TSettings : new()
        {
            var builder = new ConfigurationBuilder();
            return builder
                .AddJsonFile(configurationFileName)
                .Build()
                .Get<TSettings>();
        }
    }
}
