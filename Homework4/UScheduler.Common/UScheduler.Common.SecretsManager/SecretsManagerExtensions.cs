using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UScheduler.Common.SecretsManager.Settings;

namespace UScheduler.Common.SecretsManager
{
    public static class SecretsManagerExtensions
    {
        public static IConfigurationBuilder UseServiceManager(this IConfigurationBuilder builder)
        {
            var keyVaultSecrets = ConfigurationBinder.ConfigurationBinder.GetConfiguration<KeyVaultSettings>("Configurations/key-vault.json");
            var secretClient = new SecretClient(new Uri(keyVaultSecrets.KeyVaultUrl),
                new ClientSecretCredential(keyVaultSecrets.TenantId, keyVaultSecrets.ClientId,
                    keyVaultSecrets.ClientSecret));
            return builder.AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions());
        }

        public static IServiceCollection AddServiceManager(this IServiceCollection services)
        {
            var keyVaultSecrets = ConfigurationBinder.ConfigurationBinder.GetConfiguration<KeyVaultSettings>("Configurations/key-vault.json");
            services.AddAzureClients(builder =>
            {
                builder.AddSecretClient(new Uri(keyVaultSecrets.KeyVaultUrl));
            });
            return services.AddScoped<ISecretsManager, SecretsManager>();
        }
    }
}
