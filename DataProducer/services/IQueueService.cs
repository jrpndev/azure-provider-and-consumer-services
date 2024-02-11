using Azure.Messaging.ServiceBus;
using DataProducer.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class ServiceBusMessageSenderService : IServiceBusMessageSenderService
{
    private readonly ServiceBusSender _sender;

    public ServiceBusMessageSenderService(string QueueName, string ConnectionString)
    {
        _sender = new ServiceBusClient(ConnectionString).CreateSender(QueueName);
    }
    public async Task SendMessageAsync(string message)
    {
        await _sender.SendMessageAsync(new ServiceBusMessage(message));
    }
}
