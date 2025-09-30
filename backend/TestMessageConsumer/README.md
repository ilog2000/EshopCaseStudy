# TestMessageConsumer

A simple console application that consumes messages from RabbitMQ using MassTransit. The application listens for product-related events and displays them as formatted JSON in the console.

## Features

- Consumes `ProductCreated`, `ProductUpdated`, and `ProductDeleted` messages
- Serializes received messages to JSON format
- Displays messages with timestamps in the console
- Uses MassTransit with RabbitMQ transport

## Prerequisites

- .NET 9.0 or later
- RabbitMQ server running on `localhost:5672` with default credentials (`guest`/`guest`)

## How to Run

1. Start RabbitMQ server (if not already running):
```sh
# Using docker-compose
docker compose up -d rabbitmq
```

2. Run the consumer application:
```bash
dotnet run --project backend/TestMessageConsumer
```

3. The application will start listening for messages on the `product-events` queue and display them in the console.

## Message Types

The consumer handles the following message types from the `Core.Contracts` namespace:

- **ProductCreated**: Contains full product information when a product is created
- **ProductUpdated**: Contains updated product information
- **ProductDeleted**: Contains the ID of the deleted product

## Output Format

Each received message is displayed with:
- Timestamp
- Message type
- JSON-formatted message content
- Separator line

Example output:
```
[2025-09-30 15:30:45] Received ProductCreated message:
{
  "Id": "123e4567-e89b-12d3-a456-426614174000",
  "Name": "Sample Product",
  "ImageUrl": "https://example.com/image.jpg",
  "Description": "A sample product description",
  "Price": 29.99,
  "StockQuantity": 100,
  "CreatedAt": "2025-09-30T15:30:45Z",
  "UpdatedAt": "2025-09-30T15:30:45Z",
  "Categories": ["Electronics", "Gadgets"]
}
--------------------------------------------------
```

## Configuration

The application is configured to:
- Connect to RabbitMQ on `localhost:5672`
- Use default credentials (`guest`/`guest`)
- Listen on the `product-events` queue
- Process messages from all three consumer types

To modify the connection settings, update the RabbitMQ configuration in `Program.cs`.
