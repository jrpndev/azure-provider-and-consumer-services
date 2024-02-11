using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using DataProducer.Models;

public static class InfraDependencyInjection
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Registrando o CosmosClient
        services.AddSingleton(provider =>
        {
            var cosmosConfig = configuration.GetSection("CosmosDB");
            var endpointUri = cosmosConfig.GetValue<string>("EndpointUri");
            var authKey = cosmosConfig.GetValue<string>("AuthKey");
            var cosmosClient = new CosmosClient(endpointUri, authKey);
            return cosmosClient;
        });

        services.AddScoped<ICustomerRepository>(provider =>
        {
            var cosmosClient = provider.GetRequiredService<CosmosClient>();
            var cosmosConfig = configuration.GetSection("CosmosDB");
            var databaseName = cosmosConfig.GetValue<string>("DatabaseName");
            var containerName = cosmosConfig.GetValue<string>("ContainerName");
            return new CosmosDBCustomerRepository(cosmosClient, databaseName, containerName);
        });

        services.AddSingleton<IServiceBusMessageSenderService, ServiceBusMessageSenderService>(provider =>
        {
            var serviceBusConfig = configuration.GetSection("ServiceBus");
            var connectionString = serviceBusConfig.GetValue<string>("ConnectionString");
            var queueName = serviceBusConfig.GetValue<string>("QueueName");
            return new ServiceBusMessageSenderService(queueName, connectionString);
        });

    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        InfraDependencyInjection.RegisterServices(services, configuration);
    }
}
