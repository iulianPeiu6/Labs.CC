using Azure.Security.KeyVault.Secrets;

namespace UScheduler.Common.SecretsManager
{
    public interface ISecretsManager
    {
        Task<KeyVaultSecret> GetSecretAsync(string secretName);
    }

    internal sealed class SecretsManager : ISecretsManager
    {
        private readonly SecretClient _secretClient;

        public SecretsManager(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        public async Task<KeyVaultSecret> GetSecretAsync(string secretName)
        {
            return (await _secretClient.GetSecretAsync(secretName)).Value;
        }
    }
}
