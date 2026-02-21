# EAF Template BFF

## Overview

EAF Template BFF (Back-end for Front-end) is a .NET 10.0 template implementing BFF architectural pattern with modern best practices, SOLID principles, and Clean Architecture.

## Purpose

BFF stands for Back-end for Front-end, an architectural pattern that provides a dedicated backend component to serve data specifically optimized for different frontend applications. This results in better user experience, improved performance, and enhanced scalability.

## Features

- .NET 10.0 with latest C# features
- Clean Architecture with clear separation of concerns
- SOLID Principles implementation
- Docker Support with Alpine Linux optimization
- Comprehensive Testing with xUnit, Moq, and FluentAssertions
- Code Coverage tracking and reporting
- GitHub Actions CI/CD pipeline
- OpenTelemetry integration for observability
- Serilog structured logging
- Health Checks for monitoring

## Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Frontend     │    │   BFF API      │    │  External APIs  │
│   (React/Angular)│◄──►│   (.NET 10.0)   │◄──►│  (Bacen/Febraban)│
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### Project Structure

```
src/
├── Eaf.Template.Bff.Core/          # Domain logic and services
│   ├── Models/                     # Domain models
│   ├── Services/                   # Business logic
│   ├── Extensions/                 # Utilities and extensions
│   ├── Middlewares/                # Custom middleware
│   └── Configurations/             # Configuration classes
├── Eaf.Template.Bff.Host/         # API layer
│   ├── Controllers/                # API controllers
│   ├── Swagger/                   # API documentation
│   └── Properties/                # Configuration files
└── Eaf.Template.Bff.Proxy/        # External API clients
    └── Bacen/                     # Bacen API integration

tests/
└── Eaf.Template.Bff.Tests/        # Unit and integration tests
```

## Quick Start

### Prerequisites

- .NET 10.0 SDK
- Docker (optional, for containerized deployment)
- Git

### Running Locally

```bash
# Clone the repository
git clone https://github.com/afonsoft/bff.git
cd bff

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the application
dotnet run --project src/Eaf.Template.Bff.Host

# Run tests
dotnet test
```

### Docker Deployment

```bash
# Build the Docker image
docker build -t eaf-template-bff .

# Run the container
docker run -d \
  --name bff \
  -p 5000:5000 \
  -e DOTNET_PROCESSOR_COUNT=2 \
  eaf-template-bff
```

## Code Coverage

**Current Coverage: 5.72%** (57/995 lines covered)

### Coverage Details
- **Lines Covered**: 57
- **Lines Valid**: 995
- **Branch Coverage**: 1.82% (5/274 branches)
- **Test Status**: 12 tests passing

### Coverage by Project
- **Eaf.Template.Bff.Core**: 6.24%
- **Eaf.Template.Bff.Host**: 0% (no tests yet)
- **Eaf.Template.Bff.Proxy**: 0% (no tests yet)

## Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Run specific test project
dotnet test tests/Eaf.Template.Bff.Tests/Eaf.Template.Bff.Tests.csproj
```

### Test Structure

- **Unit Tests**: Testing individual components in isolation
- **Integration Tests**: Testing component interactions
- **BDD Style**: Behavior-Driven Development approach
- **Mocking**: Using Moq for dependency isolation
- **Assertions**: FluentAssertions for readable test code

## Configuration

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `DOTNET_PROCESSOR_COUNT` | Number of CPU cores for processing | `2` |
| `ASPNETCORE_URLS` | Application URLs | `http://+:4000` |
| `DOTNET_URLS` | .NET URLs | `http://+:4000` |
| `DOTNET_SYSTEM_GLOBALIZATION_INVARIANT` | Globalization invariant mode | `false` |

### Application Settings

- **Logging**: Serilog with structured JSON output
- **Health Checks**: `/health` endpoint
- **API Documentation**: Swagger UI at `/swagger`
- **CORS**: Configured for frontend integration

## API Endpoints

### Bacen Integration

```http
GET /api/bacen?filter={filter}
```

**Response:**
```json
{
  "success": true,
  "response": [
    {
      "compe": "001",
      "ispb": "00000000",
      "name": "Banco do Brasil S.A.",
      "code": "1",
      "fullName": "Banco do Brasil S.A."
    }
  ]
}
```

## Security

- **JWT Authentication**: Token-based authentication
- **CORS**: Cross-origin resource sharing configuration
- **Input Validation**: Request validation and sanitization
- **Error Handling**: Centralized exception handling

## Performance

- **Caching**: Distributed caching with Redis support
- **Circuit Breaker**: Resilience patterns for external APIs
- **Async/Await**: Non-blocking operations throughout
- **Optimized Docker**: Alpine Linux base image for smaller footprint

## Development

### Code Quality

- **SOLID Principles**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **Clean Architecture**: Domain, Application, Infrastructure, Presentation layers
- **Design Patterns**: Repository, Service, Factory, Strategy
- **Code Analysis**: Static analysis and linting

### Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

Please read the [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Links

- **Repository**: https://github.com/afonsoft/bff
- **Issues**: https://github.com/afonsoft/bff/issues
- **Discussions**: https://github.com/afonsoft/bff/discussions
- **Actions**: https://github.com/afonsoft/bff/actions

## Project Status

**Status**: Active Development

- **Build**: Passing
- **Tests**: 12/12 passing
- **Coverage**: 5.72% (needs improvement)
- **Code Quality**: Good
- **Documentation**: Complete

---

*Built with  using .NET 10.0 and modern development practices*
