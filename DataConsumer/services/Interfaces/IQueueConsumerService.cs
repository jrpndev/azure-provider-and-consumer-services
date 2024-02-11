using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

public interface IServiceBusMessageConsumerService : IDisposable
{
    Task ProcessMessageHandler(ProcessMessageEventArgs args);
    Task ErrorHandler(ProcessErrorEventArgs args);
    Task<IActionResult> ShowMessage();
}
