using System;

namespace Core.Contracts;

public class HealthCheck
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Healthy";
}
