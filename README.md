# Case Study of Event-Driven Micorservice Architecture

This project explores a modern approach to event-driven microservice architecture, focusing on best practices for building scalable, resilient, and loosely coupled systems. The aim is to study how events can be used to coordinate services, improve communication, and enable flexibility in enterprise applications.

## Requirements

* The solution should be based on the .NET Core framework (the latest version or latest LTS version) and C#.
* One of services should be a REST API service that provides all available products of an e-shop and allows partial updates of a single product.
* OpenAPI/Swagger documentation should be provided for all endpoints.
* The main focus should be on best practices for application design and implementation, proper architecture and modern design principles.

## Key Design Highlights

- **Microservices with Independent Storage:** Each microservice manages its own database, ensuring clear boundaries and data ownership.
- **Event-Driven Communication:** Services interact by publishing and subscribing to events, promoting loose coupling and scalability.
- **Message Bus Integration:** A central message bus enables reliable event delivery and service coordination.
- **MVC Pattern:** Microservices are built using the MVC pattern for clear separation of concerns.
- **Domain-Driven Design (DDD):** Core business entities are organized in a dedicated class library for maintainability.
- **Clean Data Access:** The `DbContext` uses the fluent API to keep domain classes free from Entity Framework Core dependencies.
- **Repository Pattern:** Data access logic is encapsulated in repositories, promoting testability and separation of concerns.
- **CQRS Pattern:** DTOs are split into "read" and "write" models to support Command Query Responsibility Segregation, improving clarity and scalability.
- **Controller and DTO Usage:** Controllers handle requests using Data Transfer Objects (DTOs) for clear data contracts.
- **Domain Service Layer:** A dedicated domain service layer maps between DTOs and domain entities, ensuring business logic is centralized and data transformations are consistent.
- **Docker Containers:** Each microservice is packaged as a Docker container for consistent deployment and easy scalability.

## Implementation Notes

### `*.http` files

These files are used for manual web API testing by sending requests and getting responses.

To use them from Visual Studio Code, install [REST Clent extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client). Visual Studio 2022 supports them out of the box.

### Taskfile.yml

Provides command line shortcuts. See https://taskfile.dev/ for installation and details.

### OpenAPI

Scalar is used to generate and serve the OpenAPI documentation for all microservice endpoints.

The UI is available on http://localhost:5292/scalar/v1
