using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Extensions.AspNetCore.Configuration.Secrets;


namespace Producer.Extensions
{
    public static class Extension
    {
        public static void ConfigureKeyVault(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var keyVaultClientId = configuration["KeyVaultConfiguration:ClientId"];
            var keyVaultTenantId = configuration["KeyVaultConfiguration:TenantId"];
            var keyVaultClientSecret = configuration["KeyVaultConfiguration:ClientSecret"];
            var keyVaultUrl = configuration["KeyVaultConfiguration:KVUrl"];

            var secretClient = new SecretClient(new Uri(keyVaultUrl),
                new ClientSecretCredential(keyVaultTenantId, keyVaultClientId, keyVaultClientSecret));

            builder.Configuration.AddAzureKeyVault(
                secretClient,
                new AzureKeyVaultConfigurationOptions()
                {
                    Manager = new KeyVaultSecretManager()
                });

        }
    }
}
