# System Documentation — EAF Template BFF

> **English (en-US)**. The Portuguese (pt-BR) version is available at [pt-BR/README.md](../pt-BR/README.md).

## Overview

EAF Template BFF is a .NET 10 Back-end for Front-end template using Clean Architecture, observability, resilience, and automated tests.

## Architecture

```
Frontend -> Host (Controllers) -> Core (Services) -> Proxy (HttpClients) -> External APIs
```

## Directory Structure

- `src/Eaf.Template.Bff.Core` — domain logic, services, cache, middleware, and extensions.
- `src/Eaf.Template.Bff.Proxy` — HTTP clients for Bacen/BCB and Febraban.
- `src/Eaf.Template.Bff.Host` — ASP.NET Core API.
- `tests/Eaf.Template.Bff.Tests` — xUnit/Moq/FluentAssertions/Shouldly/NSubstitute tests.
- `.github/workflows` — CI/CD, quality, and security pipelines.
- `.claude/` and `.devin/` — agent harness and rules.
- `CLAUDE.md` — agent harness index.

## Quick Start

```bash
dotnet restore Eaf.Template.Bff.sln
dotnet build Eaf.Template.Bff.sln
dotnet test tests/Eaf.Template.Bff.Tests/Eaf.Template.Bff.Tests.csproj
dotnet run --project src/Eaf.Template.Bff.Host
```

## Tests and Coverage

- Total tests: 37
- Line coverage: 27.91% (target: 90%)
- Branch coverage: 15.89%
- `Proxy` is at 86.11% coverage; `Core` and `Host` still need more tests.

## References

- [README.md](../../README.md) (en-US)
- [technologies.md](./technologies.md)
- [packages.md](./packages.md)
- [plugins.md](./plugins.md)
- [features.md](./features.md)
- [api.md](./api.md)
