using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class ServiceBusMessageConsumerService : IServiceBusMessageConsumerService
{
    private readonly ServiceBusProcessor _processor;

    public ServiceBusMessageConsumerService(string QueueName, string ConnectionString)
    {
        _processor = new ServiceBusClient(ConnectionString).CreateProcessor(QueueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls =  1
        });
        _processor.ProcessErrorAsync += ErrorHandler;
        _processor.ProcessMessageAsync += ProcessMessageHandler;
        _processor.StartProcessingAsync();
    }

    public async Task<IActionResult> ShowMessage()
    {
        try
        {
            var result = await GetProcessedMessageAsync(); 
            Console.WriteLine(result);
            return new JsonResult(new { Message = result });
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(500);
        }
    }

    public Task ProcessMessageHandler(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();
        Console.WriteLine(body);
        return args.CompleteMessageAsync(args.Message);
    }

    public Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    private async Task<string> GetProcessedMessageAsync()
    {
        return "Mensagem processada";
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

