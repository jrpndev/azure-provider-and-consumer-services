public interface ICustomerRepository
{
    Task AddCustomerAsync(Customer customer);
    Task<List<Customer>> GetCustomersAsync();
}
