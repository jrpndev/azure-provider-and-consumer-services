using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

public static class InfraDependencyInjection
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IServiceBusMessageConsumerService>(provider =>
        {
            var serviceBusConfig = configuration.GetSection("ServiceBus");
            var connectionString = serviceBusConfig.GetValue<string>("ConnectionString");
            var queueName = serviceBusConfig.GetValue<string>("QueueName");
            return new ServiceBusMessageConsumerService(queueName, connectionString);
        });
    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        InfraDependencyInjection.RegisterServices(services, configuration);
    }
}
