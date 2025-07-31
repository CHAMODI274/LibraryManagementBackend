# LibraryManagementBackend
A .NET Core Web API for managing a library system, featuring user authentication, book and author management, and borrowing records. Built with Entity Framework Core, JWT authentication, and SQLite.

---

## Features

- **CRUD operations** for Books, Authors, Users, and Borrow Records
- **User authentication and authorization** using JWT (JSON Web Tokens)
- **Entity Framework Core** for database access and migrations
- **Swagger UI** for API documentation and testing
- Manual API testing via Postman
- Support for **dependency injection** and clean architecture with services and repositories
- Designed for easy deployment using **Docker**

---

## Technologies Used

- ASP.NET Core Web API (.NET 9)
- Entity Framework Core
- ASP.NET Core Identity
- SQLite
- JWT Authentication
- Swagger (OpenAPI)
- Docker (Multi-stage build)
- Postman (for API testing)

---

---

## Getting Started

### Prerequisites

- [.NET SDK 7 or later](https://dotnet.microsoft.com/en-us/download)
- [SQL Server / SQLite / Your preferred database]
- [Docker](https://www.docker.com/get-started) (optional, for containerized deployment)
- [Postman](https://www.postman.com/) or any API client (optional, for testing)

### Installation & Run

1. **Clone the repository**

```bash
git clone https://github.com/CHAMODI274/LibraryManagementBackend.git
cd LibraryManagementBackend
