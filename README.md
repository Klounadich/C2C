# TradeHub Backend

Backend for a platform facilitating the sale and exchange of goods between users.

## Overview

TradeHub provides an API for:

*   user registration and authentication;
*   creating and searching for listings;
*   managing categories;
*   favorite items;
*   shopping cart;
*   listing moderation;
*   view counting;
*   payment processing.

## Tech Stack

| Technology            | Purpose                         |
| --------------------- | ------------------------------- |
| .NET 10               | Backend platform                |
| ASP.NET Core          | HTTP API                        |
| Entity Framework Core | ORM                             |
| PostgreSQL            | Main database                   |
| Redis                 | Caching and counters            |
| MediatR               | Application layer communication |
| Docker                | Containerization                |
| JWT                   | Authentication                  |

## Architecture

Backend построен по модульному принципу.

Основные модули:

* Catalog
* Users
* Favorites
* Cart
* Moderation
* Payments

Каждый модуль содержит собственную бизнес-логику и инфраструктурные зависимости.

## Requirements

Before running the project, install:

* .NET 10 SDK
* Docker
* PostgreSQL

## Running locally

Clone the repository:

```bash
git clone <repository-url>
cd C2C
```

Start infrastructure:

```bash
docker build -t .
```

Run the application:

```bash
dotnet run
```

The API will be available at:

```text
http://localhost:5000
```

## Configuration

Application configuration is stored in:

```text
appsettings.json
appsettings.Development.json
```

Sensitive values such as database passwords, JWT secrets and API keys must not be committed to the repository.

Use environment variables or user secrets for local development.

## API Documentation

API documentation is available through Swagger:

```text
http://localhost:5000/swagger/
```

Detailed API documentation:

* [Catalog](docs/Catalog.md)
* [Users](docs/Users.md)
* [Favorites](docs/Favorites.md)
* [Cart](docs/Cart.md)
* [Payments](docs/Payments.md)

## Project Structure

```text
C2C/
├── API/
├── Catalog/
├── Users/
├── Favorites/
├── Cart/
├── Payments/
├── Moderation/
└── DockerFile
```

## Development

Before creating a new module:

1. Define the module responsibility.
2. Define its public API.
3. Define commands and queries.
4. Define required database entities.
5. Implement business logic.
6. Add tests.
7. Update API documentation.

## License

Study project.
