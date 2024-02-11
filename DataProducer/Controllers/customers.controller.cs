using DataProducer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IServiceBusMessageSenderService _messageSenderService;
    private readonly string _basicAuthUsername;
    private readonly string _basicAuthPassword;

    public CustomersController(ICustomerRepository customerRepository, IServiceBusMessageSenderService messageSenderService)
    {
        _customerRepository = customerRepository;
        _messageSenderService = messageSenderService;
        _basicAuthUsername = "teste01";
        _basicAuthPassword = "asafugaz394";
    }

    [HttpPost]
    public async Task<IActionResult> AddCustomer(Customer customer)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_basicAuthUsername}:{_basicAuthPassword}")));

                HttpResponseMessage response = await client.GetAsync("http://localhost:5190/api/check");

                response.EnsureSuccessStatusCode();

                await _customerRepository.AddCustomerAsync(customer);
                await _messageSenderService.SendMessageAsync($"{customer.Id},{customer.Name},{customer.Email}");

                return Ok("Customer added successfully");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error adding customer: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        try
        {
            var customers = await _customerRepository.GetCustomersAsync();
            return Ok(customers);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error getting customers: {ex.Message}");
        }
    }
}
