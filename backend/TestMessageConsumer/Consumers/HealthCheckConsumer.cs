using System;
using System.Text.Json;
using Core.Contracts;
using MassTransit;

namespace TestMessageConsumer.Consumers;

public class HealthCheckConsumer : IConsumer<HealthCheck>
{
    public async Task Consume(ConsumeContext<HealthCheck> context)
    {
        var message = context.Message;
        var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Received HealthCheck message:");
        Console.WriteLine(json);
        Console.WriteLine(new string('-', 50));

        await Task.CompletedTask;
    }
}
