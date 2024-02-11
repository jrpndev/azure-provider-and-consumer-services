using Microsoft.Azure.Cosmos;

public class CosmosDBCustomerRepository : ICustomerRepository
{
    private readonly Container _container;

    public CosmosDBCustomerRepository(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        _container = cosmosClient.GetDatabase(databaseName).GetContainer(containerName);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _container.UpsertItemAsync(customer);
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        var query = _container.GetItemQueryIterator<Customer>(new QueryDefinition("SELECT * FROM c"));
        var results = new List<Customer>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }
}
