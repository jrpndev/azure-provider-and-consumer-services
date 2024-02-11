namespace DataProducer.Models
{
    public interface IServiceBusMessageSenderService
    {
        Task SendMessageAsync(string message);
    }
}