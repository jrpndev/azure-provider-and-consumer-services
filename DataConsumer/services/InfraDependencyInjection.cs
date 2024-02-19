using Microsoft.Extensions.Options;
public static class InfraDependencyInjection
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfigurations(configuration);

        services.AddSingleton<IServiceBusMessageConsumerService, ServiceBusMessageConsumerService>(provider =>
        {
            var serviceProvider = provider.CreateScope().ServiceProvider;
            var serviceBusApiConfig = serviceProvider.GetRequiredService<IOptionsSnapshot<ServiceBus>>().Value;
            var credentialsApiConfig = serviceProvider.GetRequiredService<IOptionsSnapshot<CredentialsAPIConfig>>().Value;
            return new ServiceBusMessageConsumerService(serviceBusApiConfig.QueueName, serviceBusApiConfig.ConnectionString);
        });
    }

    private static void AddConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceBus>(configuration.GetSection(nameof(ServiceBus)));
        services.Configure<CredentialsAPIConfig>(configuration.GetSection(nameof(CredentialsAPIConfig)));
    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        RegisterServices(services, configuration);
    }
}
