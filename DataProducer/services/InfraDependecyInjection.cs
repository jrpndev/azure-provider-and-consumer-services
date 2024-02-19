using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Extensions;

public static class InfraDependencyInjection
{
    public static async Task RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Configurando o acesso ao Azure Key Vault
        services.ConfigureKeyVault(configuration);

        // Configurando o acesso ao Cosmos DB
        await services.ConfigureCosmosDB(configuration);

        // Configurando o acesso ao Service Bus
        await services.ConfigureServiceBus(configuration);
    }

    public static async Task ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        await InfraDependencyInjection.RegisterServices(services, configuration);
    }
}
