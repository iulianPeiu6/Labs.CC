namespace UScheduler.Common.SecretsManager.Settings
{
    public sealed class KeyVaultSettings
    {
        public string KeyVaultUrl { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
