using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using DataProducer.Models;
using Microsoft.Extensions.Options;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;

public static class InfraDependencyInjection
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Adicionando configurações do Azure Key Vault
        services.AddConfigurations(configuration);

        services.AddApplicationInsightsTelemetry(configuration["ApplicationInsights:InstrumentationKey"]);
        // Registrando o CosmosClient
        services.AddSingleton(provider =>
        {
            var serviceProvider = provider.CreateScope().ServiceProvider;
            var cosmosConfig = serviceProvider.GetRequiredService<IOptionsSnapshot<CosmosDB>>().Value;
            var cosmosClient = new CosmosClient(cosmosConfig.EndpointUri, cosmosConfig.AuthKey);
            return cosmosClient;
        });

        services.AddScoped<ICustomerRepository>(provider =>
        {
            var serviceProvider = provider.CreateScope().ServiceProvider;
            var cosmosConfig = serviceProvider.GetRequiredService<IOptionsSnapshot<CosmosDB>>().Value;
            var cosmosClient = provider.GetRequiredService<CosmosClient>();
            return new CosmosDBCustomerRepository(cosmosClient, cosmosConfig.DatabaseName, cosmosConfig.ContainerName);
        });

        services.AddSingleton<IServiceBusMessageSenderService, ServiceBusMessageSenderService>(provider =>
        {
            var serviceProvider = provider.CreateScope().ServiceProvider;
            var serviceBusConfig = serviceProvider.GetRequiredService<IOptionsSnapshot<ServiceBus>>().Value;
            return new ServiceBusMessageSenderService(serviceBusConfig.QueueName, serviceBusConfig.ConnectionString);
        });
    }

    private static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CosmosDB>(configuration.GetSection(nameof(CosmosDB)));
        services.Configure<ServiceBus>(configuration.GetSection(nameof(ServiceBus)));
    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        RegisterServices(services, configuration);
    }
}
