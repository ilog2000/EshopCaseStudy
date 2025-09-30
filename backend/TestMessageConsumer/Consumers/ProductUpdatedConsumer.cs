using System.Text.Json;
using Core.Contracts;
using MassTransit;

namespace TestMessageConsumer.Consumers;

public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
{
    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        var message = context.Message;
        var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Received ProductUpdated message:");
        Console.WriteLine(json);
        Console.WriteLine(new string('-', 50));

        await Task.CompletedTask;
    }
}
