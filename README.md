[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nsschultz_fantasy-baseball-position&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=nsschultz_fantasy-baseball-position)

# Fantasy Baseball Position Service

A read-only ASP.NET Core Web API microservice that serves as the source of truth for position information in the fantasy baseball ecosystem. This service provides comprehensive position data including batter and pitcher positions, position hierarchies, and eligible position transitions.

## Overview

The Position Service is a foundational microservice used by multiple downstream services to retrieve accurate, up-to-date position information. As a read-only service, it focuses on **performance, reliability, and backward compatibility**.

### Key Characteristics
- **Read-only API**: All endpoints are GET operations
- **Foundation Service**: Used by other fantasy baseball microservices
- **Database-backed**: PostgreSQL provides the source of truth
- **Containerized**: Docker support for cloud deployment
- **Well-documented**: OpenAPI/Swagger integration for API exploration

---

## Prerequisites

- **.NET SDK**: .NET 10.0 or later
- **Docker**: For containerized development and runtime environments
- **PostgreSQL**: For database connectivity (when running outside containers)
- **VS Code**: Recommended IDE with DevContainer support

---

## Setup Instructions

### Local Development

1. **Clone the repository:**
   ```bash
   git clone https://github.com/nsschultz/fantasy-baseball-position.git
   cd fantasy-baseball-position
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the solution:**
   ```bash
   dotnet build
   ```

4. **Run tests:**
   ```bash
   dotnet test
   ```

### Development Container

VS Code DevContainer setup is provided for consistent development environments:

1. In VS Code, use the **"Clone Repository into Container Volume..."** option
2. Or open the workspace folder and select **"Reopen in Container"** when prompted
3. Tasks are pre-configured in `tasks.json` for common operations

---

## Building and Running

### Build the Service

```bash
dotnet build
```

### Run Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test FantasyBaseball.PositionService.UnitTests
dotnet test FantasyBaseball.PositionService.IntegrationTests
```

### Local Execution

```bash
# Run the service locally
cd FantasyBaseball.PositionService
dotnet run
```

### Docker Image

**Build:**
```bash
version=$(cat version.txt) && docker build -t nschultz/fantasy-baseball-position:$version .
```

**Runtime Containers:**
```bash
# Start containers
docker compose -f .docker-compose/docker-compose-runtime.yaml -p fantasy-baseball-position up --build -d

# Stop containers
docker compose -f .docker-compose/docker-compose-runtime.yaml -p fantasy-baseball-position down
```

---

## Architecture

### Project Structure

```
FantasyBaseball.PositionService/
├── Controllers/           - ASP.NET Core API endpoints
├── Database/
│   ├── Entities/         - Domain models (Position, AdditionalPosition)
│   ├── Repositories/     - Data access layer
│   ├── Migrations/       - EF Core schema migrations
│   └── PositionContext   - Entity Framework DbContext
├── Services/             - Business logic layer
├── Maps/                 - AutoMapper profiles for DTO mapping
├── Program.cs            - DI configuration and middleware setup
└── appsettings.json      - Configuration

FantasyBaseball.PositionService.UnitTests/
└── Services/             - Unit tests for services and utilities

FantasyBaseball.PositionService.IntegrationTests/
└── PositionIntegrationTests - End-to-end API tests
```

### Key Design Patterns

- **Dependency Injection**: Constructor-based DI via ASP.NET Core
- **Repository Pattern**: IPositionRepository abstracts data access
- **Async/Await**: All I/O operations are async
- **Mapper Pattern**: AutoMapper handles Entity-to-DTO conversions
- **Health Checks**: Database connectivity monitoring

---

## API Endpoints

### Get All Positions

```
GET /api/v1/position
```

Returns a JSON array of all positions sorted by SortOrder.

**Example Response:**
```json
[
  {
    "code": "C",
    "fullName": "Catcher",
    "playerType": 1,
    "sortOrder": 0,
    "additionalPositions": []
  },
  ...
]
```

### Health Check

```
GET /api/health
```

Returns the service health status. Fails if database is inaccessible.

### API Documentation

```
GET /api/swagger/index.html
```

Interactive Swagger UI for exploring and testing API endpoints.

---

## Local Development Access

When running the service locally via Docker Compose:

| Service | URL |
|---------|-----|
| Position API / Swagger | http://localhost:8080/api/swagger/index.html |
| PgAdmin (Database GUI) | http://localhost:9000 |
| PostgreSQL Database | `database:5432` (container network only) |

---

## Code Quality and Testing

### Test Framework

- **Framework**: xUnit
- **Assertion Library**: xUnit.Assert with FluentAssertions patterns
- **Coverage**: 25+ tests covering unit and integration scenarios

### Test Structure

- **Unit Tests** (`FantasyBaseball.PositionService.UnitTests`): Isolated tests for services, repositories, and entities
- **Integration Tests** (`FantasyBaseball.PositionService.IntegrationTests`): End-to-end API testing with real database

### Running Tests with Coverage

```bash
# Install coverage tool (one-time)
dotnet tool install -g dotnet-coverage

# Generate coverage report
dotnet-coverage collect -f cobertura -o coverage.cobertura.xml dotnet test
```

### Pre-Commit Hooks

This repository includes automated pre-commit hooks to maintain code quality standards:

**Installation:**
```bash
# Automatic configuration
git config core.hooksPath .githooks
```

**What it validates:**
- 🔒 **Secrets Detection**: Prevents accidental commit of passwords, tokens, API keys
- 🎨 **Code Formatting**: Enforces consistent code style via `dotnet format`
- 📦 **File Size Check**: Prevents large files (> 5MB) from being committed
- 🔨 **Build Check** (optional): Can be enabled to verify build succeeds
- ✅ **Test Check** (optional): Can be enabled to verify tests pass

**Bypass hook if needed:**
```bash
git commit --no-verify
```

### Build Status

- **Compilation**: 0 errors, 0 warnings
- **Tests**: All passing (16 unit + 9 integration = 25 total)
- **Security**: No known vulnerabilities

### CI/CD Workflows

This project uses GitHub Actions for automated quality assurance:

**PR Validation** (`.github/workflows/pr-validation.yml`):
- Triggers on every pull request (opened, synchronize, reopened)
- **Build & Test**: Compiles code and runs test suite with result reporting
- **SonarCloud Quality Gate**: Verifies code quality metrics and coverage ≥ 80%
- **Code Format**: Ensures consistent code style via `dotnet format`
- **Security Scan**: Scans with Trivy for vulnerabilities

**Release Build** (`.github/workflows/docker-image.yml`):
- Triggers on version tags matching `v*.*.*`
- Builds multi-platform Docker images (amd64, arm64)
- Pushes both versioned and `latest` images to Docker Hub

---

## C# Best Practices

This project adheres to the C# best practices defined in [.github/agents/CSharpExpert.agent.md](.github/agents/CSharpExpert.agent.md), including:

- ✅ XML documentation for all public APIs
- ✅ Async/await throughout the stack
- ✅ Proper null-checking with ArgumentNullException
- ✅ Descriptive exception types and error handling
- ✅ Dependency injection for testability
- ✅ Comprehensive unit and integration tests
- ✅ SOLID principles and design patterns

---

## Configuration

### Environment Variables

Required environment variables for database connectivity:

- `POSITION_DATABASE_HOST`: PostgreSQL server host
- `POSITION_DATABASE`: Database name
- `POSITION_DATABASE_USER`: Database user
- `POSITION_DATABASE_PASSWORD`: Database password

### Connection String

Default connection string in `appsettings.json`:
```
Server={POSITION_DATABASE_HOST};Database={POSITION_DATABASE};User Id={POSITION_DATABASE_USER};Password={POSITION_DATABASE_PASSWORD};
```

---

## Contributing Guidelines

When contributing to this project:

1. **Follow C# Best Practices**: Reference [.github/agents/CSharpExpert.agent.md](.github/agents/CSharpExpert.agent.md)
2. **Maintain Backward Compatibility**: This is a foundational service used by multiple consumers
3. **Add Tests**: All new public APIs must include corresponding unit and/or integration tests
4. **Document Changes**: Update this README and XML documentation as needed
5. **Run Full Test Suite**: Ensure all 25+ tests pass before submitting changes
6. **Code Review**: All changes require code review before merging

### Development Workflow

1. Create a feature branch: `git checkout -b feature/your-feature`
2. Make changes following best practices
3. Run tests: `dotnet test`
4. Commit with descriptive messages
5. Push to your fork and create a Pull Request

---

## Troubleshooting

### Database Connection Issues

If the service fails health checks:
1. Verify PostgreSQL is running and accessible
2. Check environment variables are set correctly
3. Verify database user has appropriate permissions
4. Check logs for specific connection error details

### Build Failures

1. Ensure .NET 10.0 SDK is installed: `dotnet --version`
2. Restore packages: `dotnet restore`
3. Clean build: `dotnet clean && dotnet build`

### Test Failures

1. Run tests individually to isolate issues: `dotnet test --filter NameOfTest`
2. Check for side effects or test ordering issues
3. Verify database state for integration tests

---

## Additional Resources

- [Microsoft .NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [AutoMapper Documentation](https://docs.automapper.org/)
- [xUnit Documentation](https://xunit.net/)

---

## Support

For issues, questions, or contributions, please:
1. Check existing GitHub issues and discussions
2. Create a new GitHub issue with detailed information
3. Reference relevant code sections and error messages
