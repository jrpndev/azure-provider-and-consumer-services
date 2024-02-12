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

    public CustomersController(ICustomerRepository customerRepository, IServiceBusMessageSenderService messageSenderService)
    {
        _customerRepository = customerRepository;
        _messageSenderService = messageSenderService;

    }

    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerDTO customer, [FromHeader(Name = "Authorization")] string authorizationHeader)
    {
        if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            return new UnauthorizedResult();
        }

        var encodedUsernamePassword = authorizationHeader.Substring("Basic ".Length).Trim();
        var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

        var parts = decodedUsernamePassword.Split(':', 2);
        var username = parts[0];
        var password = parts[1];

        try
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

                HttpResponseMessage response = await client.GetAsync("http://localhost:5190/api/check");

                if (response.IsSuccessStatusCode)
                {
                    await _customerRepository.AddCustomerAsync(customer);
                    await _messageSenderService.SendMessageAsync($"{customer.Id},{customer.Name},{customer.Email}");

                    return Ok("Customer added successfully");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Error calling external service: {response.ReasonPhrase}");
                }
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
