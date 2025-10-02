using System;
using Core.Contracts;
using MassTransit;

namespace ProductService.Services;

public class HealthCheckPublisher
{
    private readonly IBus _bus;

    public HealthCheckPublisher(IBus bus)
    {
        _bus = bus;
    }

    public Task PublishHealthCheckAsync()
    {
        return _bus.Publish(new HealthCheck(), context => context.Durable = false);
    }
}

