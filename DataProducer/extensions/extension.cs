using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Extensions
{
    public static class Extension
    {
        public static SecretClient CreateSecretClient(IConfiguration configuration)
        {
            var keyVaultClientId = configuration["KeyVaultConfiguration:ClientId"];
            var keyVaultTenantId = configuration["KeyVaultConfiguration:TenantId"];
            var keyVaultClientSecret = configuration["KeyVaultConfiguration:ClientSecretId"];
            var keyVaultUrl = configuration["KeyVaultConfiguration:KVUrl"];

            return new SecretClient(new Uri(keyVaultUrl),
                new ClientSecretCredential(keyVaultTenantId, keyVaultClientId, keyVaultClientSecret));
        }

        public static void ConfigureKeyVault(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var secretClient = CreateSecretClient(configuration);

            builder.Configuration.AddAzureKeyVault(
                secretClient,
                new AzureKeyVaultConfigurationOptions()
                {
                    Manager = new KeyVaultSecretManager()
                });
        }

        public static async Task ConfigureCosmosDB(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var secretClient = CreateSecretClient(configuration);
            var cosmosDbAuthKey = await secretClient.GetSecretAsync("vCosmosDB--AuthKey");
            var cosmosDbEndpointUri = await secretClient.GetSecretAsync("vCosmosDB--EndpointUri");
            var cosmosDbDatabaseName = await secretClient.GetSecretAsync("vCosmosDB--DatabaseName");
            var cosmosDbContainerName = await secretClient.GetSecretAsync("vCosmosDB--ContainerName");
            var cosmosDbConnectionString = $"AccountEndpoint={cosmosDbEndpointUri};AccountKey={cosmosDbAuthKey};Database={cosmosDbDatabaseName};";

        }

        public static async Task ConfigureServiceBus(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var secretClient = CreateSecretClient(configuration);

            var serviceBusConnectionString = await secretClient.GetSecretAsync("ServiceBus--ConnectionString");
            var serviceBusNamespaceName = await secretClient.GetSecretAsync("ServiceBus--NamespaceName");
            var serviceBusQueueName = await secretClient.GetSecretAsync("ServiceBus--QueueName");
        }
    }
}
