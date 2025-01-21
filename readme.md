# Developer Evaluation Project
 
`READ CAREFULLY`
 
## Instructions
**The test below will have up to 7 calendar days to be delivered from the date of receipt of this manual.**
 
- The code must be versioned in a public Github repository and a link must be sent for evaluation once completed
- Upload this template to your repository and start working from it
- Read the instructions carefully and make sure all requirements are being addressed
- The repository must provide instructions on how to configure, execute and test the project
- Documentation and overall organization will also be taken into consideration
 
## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**
 
As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.
 
Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:
 
* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled
 
It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled
 
If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.
 
### Business Rules
 
* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount
 
These business rules define quantity-based discounting tiers and limitations:
 
1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount
 
2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items
 
## Solution

To meet the key competencies evaluated throughout the process, a solution was designed using the **CQRS (Command Query Responsibility Segregation) pattern**. This approach separates read and write operations to improve performance and scalability. The system uses **MediatR** to facilitate communication between components and **PostgreSQL** as the database for its reliability and performance.

**Key Points**:

- CQRS pattern for separating read and write operations
- MediatR for decoupling components and simplifying communication
- PostgreSQL as the relational database system
- .NET 9 framework for performance and advanced features

Additionally, **RabbitMQ** was implemented for asynchronous messaging between microservices, enabling event-driven communication. The application uses **Entity Framework Core (EF Core)** for seamless database interaction, ensuring efficient CRUD operations and relationship management.

**Key Points**:

- RabbitMQ for messaging and event-driven architecture
- EF Core ORM for database interaction and query execution

The application follows key software design principles:

- **Inversion of Control (IoC)** to decouple components and enhance testability
- **Dependency Injection (DI)** for easy management of dependencies
- **Domain-Driven Design (DDD)** to align the system with business logic
- **Event-Driven Design (EDD)** for reactive and flexible workflows

To simplify API consumption, **Refit** was used, enabling quick integration with external APIs. This tool accelerates development by defining strongly-typed interfaces for external services.

**Key Points**:

**Refit** for simplified and fast external API integration

For error handling, a custom middleware was created to manage exceptions and provide consistent error responses across the application. Resource files (.resx) were used for centralized text and error codes, and a static class was implemented for constants, improving maintainability.

**Key Points**:

- Custom **middleware** for consistent exception handling
- **Resource (.resx)** files for centralized messages and error codes
- **Static class** for managing constants

The application is structured into layers:

**Api**: Handles HTTP requests and routes them to the appropriate services
**Application**: Contains the business logic
**Domain**: Holds the business model and core entities
**Infra.Data**: Manages data access
**Infra.IoC**: Configures the IoC container for dependency management

A Postman collection with essential API requests is available in the docs folder for easy testing, see the [Postman Collection](./docs/Developer%20Store.postman_collection.json).

For more details, see the [Sales API documentation](./docs/sales-api.md).

## How to Configure the Application

1. **Set up Docker Containers**: Inside the `building blocks` folder, there is a `docker-compose.yml` file. This file should be executed before running the application for the first time to set up both **RabbitMQ** and the **PostgreSQL** database containers.

- Navigate to the building blocks folder in your terminal.
- Run the following command to start the Docker containers:

   ```bash
   docker-compose up -d
   ```

This will start both the RabbitMQ and PostgreSQL containers in the background.

2. **Ensure Containers are Running**: After executing the `docker-compose` command, verify that both containers are up and running. You can check the status with:

```bash
docker ps
```

This will list the running containers. Ensure that RabbitMQ and PostgreSQL are among them.

3. **Running the Application**: Once the containers are running, you can start the application. You have two options:

- **Using .NET CLI**: Navigate to the root folder of the application and run the following command:

   ```bash
   dotnet run
   ```

- **Using Visual Studio or Rider**: Open the project in **Visual Studio** or **Rider** and run the application directly from the IDE.

This will start the application and it should be able to connect to RabbitMQ and PostgreSQL without any issues.

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates.
 
See [Overview](./docs/overview.md)
 
## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components.
 
See [Tech Stack](./docs/tech-stack.md)
 
## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability.
 
See [Frameworks](./docs/frameworks.md)
 
<!--
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)
-->
 
## Project Structure
This section describes the overall structure and organization of the project files and directories.
 
See [Project Structure](./docs/project-structure.md)
tem menu de contexto