public interface ICustomerRepository
{
    Task<bool> AddCustomerAsync(CustomerDTO customer);
    Task<List<CustomerDTO>> GetCustomersAsync();
}
