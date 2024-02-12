using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(BasicAuthorizationFilter))]
[ServiceFilter(typeof(ExecutionTimeActionFilter))]
public class CheckController : ControllerBase
{
    private readonly IServiceBusMessageConsumerService _messageConsumerService;

    public CheckController(IServiceBusMessageConsumerService messageConsumerService)
    {
        _messageConsumerService = messageConsumerService ?? throw new ArgumentNullException(nameof(messageConsumerService));
    }

    [HttpGet]
    public async Task<IActionResult> CheckMessage()
    {
        try
        {
            var result = await _messageConsumerService.ShowMessage();
            return result;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
