using Microsoft.Azure.Cosmos;

public class CosmosDBCustomerRepository : ICustomerRepository
{
    private readonly Container _container;

    public CosmosDBCustomerRepository(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        _container = cosmosClient.GetDatabase(databaseName).GetContainer(containerName);
    }

    public async Task<bool> AddCustomerAsync(CustomerDTO customer)
    {
        try
        {
            await _container.UpsertItemAsync(customer);
            return true; // If the upsert operation succeeds, return true indicating success.
        }
        catch (Exception ex)
        {
            // Log the exception here if needed
            return false; // If an exception occurs during the upsert operation, return false indicating failure.
        }
    }

    public async Task<List<CustomerDTO>> GetCustomersAsync()
    {
        var query = _container.GetItemQueryIterator<CustomerDTO>(new QueryDefinition("SELECT * FROM c"));
        var results = new List<CustomerDTO>();

        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.ToList());
        }

        return results;
    }
}
