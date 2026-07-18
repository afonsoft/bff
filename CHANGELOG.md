# Changelog

Todas as mudanças notáveis deste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/),
e este projeto adere ao [Semantic Versioning](https://semver.org/lang/pt-BR/).

## [Unreleased]

### Added
- Criação do harness de agentes para Devin e Claude Code (`.claude/`, `.devin/`, `CLAUDE.md`).
- Documentação complementar bilíngue reorganizada em `docs/en-US/` e `docs/pt-BR/` (`README.md`, `technologies.md`, `packages.md`, `features.md`, `api.md`, `plugins.md`).
- Novos testes unitários:
  - `BacenServiceTests` — cobertura de fallback Febraban/BCB e exceção agregada.
  - `BcbClientTests` e `FebrabanClientTests` — respostas válidas, vazias e erros HTTP.
  - `CacheManagerTests` — Get/Set, GetOrCreateAsync e TryGetValue.
  - `BacenControllerTests` — retorno padronizado `ApiResponse<T>`.
  - `MappingProfileTests` — mapeamento de `FebrabanBank` e `BcbBank` para `BankDto`.
  - `ExceptionExtensionsTests` — `FormatException`, `RemovePostFix` e `Left`.
- Helpers de teste: `FakeCacheManager`, `FakeDistributedCache` e `MockHttpMessageHandler`.
- Pacotes de teste `Shouldly` 4.3.0 e `NSubstitute` 6.0.0 no projeto de testes.

### Changed
- Atualização de pacotes NuGet para remover vulnerabilidades e manter compatibilidade com .NET 10:
  - `AutoMapper` 16.0.0 → 16.2.0
  - `Microsoft.AspNetCore.Authentication.JwtBearer` 9.0.3 → 10.0.10
  - `Microsoft.Extensions.Configuration.Abstractions` 10.0.3 → 10.0.10
  - `Microsoft.Extensions.Http.Resilience` 9.3.0 → 10.8.0
  - `Microsoft.Extensions.ServiceDiscovery` 9.1.0 → 10.8.0
  - `Microsoft.OpenApi` 2.7.5 → 3.9.0
  - `Newtonsoft.Json` 13.0.3 → 13.0.4
  - `OpenTelemetry.*` 1.15.0 → 1.17.0
  - `Serilog.Settings.Configuration` 10.0.1-dev-02330 → 10.0.1
  - `Swashbuckle.AspNetCore.*` 10.1.4 → 10.2.3
  - `System.Security.Cryptography.Pkcs` 9.0.3 → 10.0.10
  - Pacotes de teste atualizados para `Microsoft.NET.Test.Sdk` 18.8.1, `xunit.runner.visualstudio` 3.1.5, `coverlet.collector` 10.0.1 e `FluentAssertions` 8.10.0.
  - Referências diretas adicionadas para atualizar pacotes transitivos: `Hangfire.Core` 1.8.24, `Microsoft.Bcl.Cryptography` 10.0.10, `Microsoft.Extensions.Configuration.EnvironmentVariables` 10.0.10, `Microsoft.Extensions.DependencyModel` 10.0.10, `Polly.Core/Extensions/RateLimiting` 8.7.0, `Serilog` 4.4.0 e `System.Threading.RateLimiting` 10.0.10.
- `LangVersion` atualizado para `14.0` em todos os projetos.
- Reescrita dos workflows do GitHub Actions baseada em `afonsoft/metar-decoder` e `afonsoft/QRCoder.Core`, adaptada para a solução `Eaf.Template.Bff.sln`:
  - `ci-build-test.yml` — build, testes, formatação, cobertura e validação de vulnerabilidades; adicionados gatilhos `devin/*` e `workflow_dispatch`.
  - `code-quality.yml` — Qodana, SonarQube, Snyk e métricas de qualidade.
  - `security-scan.yml` — CodeQL, Snyk e SonarQube em agendamento; ações `github/codeql-action` atualizadas para v4.
  - `deploy-publish.yml` — ajustado para disparar apenas em tags `v*` e releases publicadas, substituído `actions/create-release` por `softprops/action-gh-release`, e adicionado `environment: production` ao job de build Docker.
- `README.md` reescrito em inglês (en-US) como padrão; `docs/README.md` é o índice em inglês e `docs/pt-BR/README.md` a tradução complementar em português.

### Fixed
- Correção de warnings de build:
  - `BcbClient.cs` — propriedades `Content` e `Sort` inicializadas para evitar warnings de null.
  - `FebrabanClient.cs` — `_baseUrl` inicializado com `string.Empty` e validação de `HttpClient` nulo.
  - `ExceptionExtensions.cs` — assinatura `string?` para `RemovePostFix` compatível com nullable references.
- Conflito de namespace `HttpClient` nos testes: arquivos de client HTTP movidos para `tests/Features/Clients`.

### Removed
- Removido `auto-pr-from-main.yml` para evitar criação automática de PRs e atualizações incorretas de pacotes.

## [0.0.1] - 2026-07-18

### Added
- Estrutura inicial do template EAF BFF com .NET 10, Clean Architecture e Docker.
- Integração com APIs Bacen/BCB e Febraban.
- Cache distribuído com compressão GZip.
- Pipeline inicial de CI/CD com GitHub Actions.

[Unreleased]: https://github.com/afonsoft/bff/compare/v0.0.1...HEAD
[0.0.1]: https://github.com/afonsoft/bff/releases/tag/v0.0.1
