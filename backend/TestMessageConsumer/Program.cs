using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TestMessageConsumer.Consumers;

var builder = Host.CreateApplicationBuilder(args);

// Configure logging
builder.Services.AddLogging(configure => configure.AddConsole());

// Configure MassTransit
builder.Services.AddMassTransit(x =>
{
    // Add consumers
    x.AddConsumer<HealthCheckConsumer>();
    x.AddConsumer<ProductCreatedConsumer>();
    x.AddConsumer<ProductUpdatedConsumer>();
    x.AddConsumer<ProductDeletedConsumer>();

    // Configure RabbitMQ transport
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        // Configure endpoints
        cfg.ReceiveEndpoint("product-events", e =>
        {
            e.ConfigureConsumer<HealthCheckConsumer>(context);
            e.ConfigureConsumer<ProductCreatedConsumer>(context);
            e.ConfigureConsumer<ProductUpdatedConsumer>(context);
            e.ConfigureConsumer<ProductDeletedConsumer>(context);
        });
    });
});

var host = builder.Build();

Console.WriteLine("TestMessageConsumer is starting...");
Console.WriteLine("Press Ctrl+C to stop the consumer");
Console.WriteLine("Listening for messages on 'product-events' queue...");
Console.WriteLine(new string('=', 60));

try
{
    await host.RunAsync();
}
catch (OperationCanceledException)
{
    Console.WriteLine("\nConsumer stopped.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
