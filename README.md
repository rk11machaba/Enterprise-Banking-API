# Enterprise Banking API

A production-ready banking backend built with ASP.NET Core 9 following Clean Architecture principles.

## Tech Stack

- **Framework:** ASP.NET Core 9
- **Database:** SQL Server with Entity Framework Core
- **Authentication:** JWT Bearer tokens
- **Password Hashing:** BCrypt
- **API Documentation:** Swagger/OpenAPI

## Architecture

```
src/
├── EnterpriseBankingAPI.API            # Controllers, Program.cs
├── EnterpriseBankingAPI.Application    # DTOs, Interfaces, Business contracts
├── EnterpriseBankingAPI.Domain         # Entities, Enums, Domain logic
└── EnterpriseBankingAPI.Infrastructure # EF Core, Services, External concerns
```

**Dependency Flow:** API → Application ← Infrastructure → Domain

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server (LocalDB or full instance)

### Configuration

Update `appsettings.json` with your connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EnterpriseBankingDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Secret": "YOUR_SECRET_KEY_MIN_32_CHARS",
    "Issuer": "EnterpriseBankingAPI",
    "Audience": "EnterpriseBankingAPIUsers"
  }
}
```

### Run Migrations

```bash
cd src/EnterpriseBankingAPI.API
dotnet ef database update --project ../EnterpriseBankingAPI.Infrastructure
```

### Run the API

```bash
dotnet run
```

Swagger UI: `https://localhost:{port}/swagger`

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register new user |
| POST | `/api/auth/login` | Login and get JWT token |

### Bank Accounts (Requires Authentication)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/accounts` | Create new account |
| GET | `/api/accounts` | Get user's accounts |
| GET | `/api/accounts/{id}` | Get specific account |
| POST | `/api/accounts/{id}/deposit` | Deposit funds |
| POST | `/api/accounts/{id}/withdraw` | Withdraw funds |
| POST | `/api/accounts/transfer` | Transfer between accounts |

## Request Examples

### Register
```json
POST /api/auth/register
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "password": "SecurePassword123"
}
```

### Login
```json
POST /api/auth/login
{
  "email": "john@example.com",
  "password": "SecurePassword123"
}
```

### Create Account
```json
POST /api/accounts
Authorization: Bearer {token}
{
  "currency": "ZAR"
}
```

### Deposit
```json
POST /api/accounts/{accountId}/deposit
Authorization: Bearer {token}
{
  "amount": 1000.00,
  "reference": "Initial deposit"
}
```

### Transfer
```json
POST /api/accounts/transfer
Authorization: Bearer {token}
{
  "fromAccountId": "guid",
  "toAccountId": "guid",
  "amount": 500.00,
  "reference": "Payment"
}
```

## Security Features

- JWT authentication with configurable expiry
- BCrypt password hashing
- User ownership validation on all account operations
- Overdraft protection
- Database transactions for transfers
- HTTPS enforcement in production

