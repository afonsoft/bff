# EAF Template BFF

![CI Build & Test](https://github.com/afonsoft/bff/actions/workflows/ci-build-test.yml/badge.svg)
![Security Scan](https://github.com/afonsoft/bff/actions/workflows/security-scan.yml/badge.svg)
![Code Quality](https://github.com/afonsoft/bff/actions/workflows/code-quality.yml/badge.svg)
![.NET 10](https://img.shields.io/badge/.NET-10.0-purple)
![License](https://img.shields.io/github/license/afonsoft/bff)

## Descrição do Projeto

O **EAF Template BFF** é um template .NET 10 de **Back-end for Front-end (BFF)** que expõe dados normalizados de instituições financeiras (Bacen, BCB e Febraban) para aplicações frontend. Ele aplica **Clean Architecture**, princípios **SOLID** e padrões de resiliência, oferecendo um ponto único, seguro e cacheável para consumo das APIs externas.

- Centraliza a comunicação com APIs legadas e instáveis.
- Aplica fallback entre Febraban (fonte primária) e BCB (fonte secundária).
- Usa cache distribuído compactado para reduzir latência e carga nos serviços externos.
- Fornece observabilidade via OpenTelemetry e logs estruturados com Serilog.

## Estrutura do Repositório

```
.
├── src/
│   ├── Eaf.Template.Bff.Core/        # Domínio, serviços, cache, middlewares, extensões
│   ├── Eaf.Template.Bff.Proxy/       # Clientes HTTP para Bacen/BCB e Febraban
│   └── Eaf.Template.Bff.Host/        # API ASP.NET Core, controllers e Swagger
├── tests/
│   └── Eaf.Template.Bff.Tests/       # Testes xUnit com Moq, FluentAssertions, Shouldly e NSubstitute
├── .github/workflows/                 # CI/CD, qualidade e segurança
├── docs/                              # Documentação complementar em português
├── .claude/                           # Configurações e regras do harness para Claude Code
├── .devin/                            # Configurações e hooks do harness para Devin
├── CLAUDE.md                          # Índice do harness para agentes
├── README.md                          # Este arquivo
└── CHANGELOG.md                       # Histórico de mudanças
```

### Detalhamento por Camada

- **`src/Eaf.Template.Bff.Core`** — Lógica de domínio, serviços (`BacenService`), cache (`CacheManager`), middleware de tratamento de exceções, extensões de DI, perfis do AutoMapper e configuração de OpenTelemetry/Serilog.
- **`src/Eaf.Template.Bff.Proxy`** — Clientes `FebrabanClient` e `BcbClient` com normalização de `BaseUrl`, serialização Newtonsoft.Json e tratamento de respostas JSON específicas.
- **`src/Eaf.Template.Bff.Host`** — Controllers (`BacenController`, `BaseController`), `Startup`, `Program` e Swagger.
- **`tests/Eaf.Template.Bff.Tests`** — Testes organizados em `Features/{Cache,Clients,Controllers,Extensions,Mappings,Models,Services}` com helpers fakes (`FakeCacheManager`, `FakeDistributedCache`, `MockHttpMessageHandler`).

## Stack Tecnológica

| Camada | Tecnologia |
|--------|-----------|
| Runtime | .NET 10.0 / C# 14 |
| Web API | ASP.NET Core |
| Mapeamento | AutoMapper 16.2.0 |
| Logs | Serilog (Console, ASP.NET Core, Settings Configuration) |
| Observabilidade | OpenTelemetry 1.17.0 (ASP.NET Core, Http, Runtime, OTLP, Prometheus) |
| Resiliência | Microsoft.Extensions.Http.Resilience 10.8.0 |
| Cache | IDistributedCache com compressão GZip |
| Autenticação | JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer 10.0.10) |
| Documentação API | Swashbuckle.AspNetCore 10.2.3 + Microsoft.OpenApi 2.7.5 |
| Serialização | Newtonsoft.Json 13.0.4 |
| Testes | xUnit, Moq, FluentAssertions 8.10.0, Shouldly 4.3.0, NSubstitute 6.0.0 |
| CI/CD | GitHub Actions |
| Qualidade | SonarCloud/SonarQube, Snyk, CodeQL, Qodana |
| Container | Docker (Alpine Linux) |

## Arquitetura

O projeto segue **Clean Architecture** com três camadas principais:

1. **Core (Domínio/Aplicação)** — Independente de frameworks externos. Contém regras de negócio, serviços, contratos (`IBacenService`, `ICacheManager`) e models.
2. **Proxy (Infraestrutura)** — Clientes HTTP e adaptadores para APIs externas. Depende apenas de bibliotecas de infraestrutura.
3. **Host (Apresentação)** — API ASP.NET Core, controllers, middlewares e configuração de DI. Depende de `Core` e `Proxy`.

### Padrões Aplicados

- **BFF (Back-end for Front-end)** — Endpoint dedicado que agrega e adapta dados para o frontend.
- **DDD** — Separação entre entidades, value objects, serviços de domínio e repositórios (quando aplicável).
- **SOLID** — Injeção de dependência via construtores, responsabilidade única e inversão de dependência.
- **Resiliência** — Fallback entre Febraban e BCB; cache com expiração deslizante e absoluta.
- **Observabilidade** — OpenTelemetry + Serilog para tracing, métricas e logs estruturados.

## Fluxo do Sistema

```mermaid
sequenceDiagram
    participant FE as Frontend
    participant BC as BacenController
    participant BS as BacenService
    participant CM as CacheManager
    participant FC as FebrabanClient
    participant BCb as BcbClient
    participant FEB as Febraban API
    participant BCB as BCB API

    FE->>BC: GET /api/bacen?filter=
    BC->>BS: GetBanksAsync(filter)
    BS->>CM: GetOrCreateAsync("febrabanClientCache_...")
    CM->>FC: GetBankAsync(filter)
    FC->>FEB: POST /Associado/Index
    FEB-->>FC: listaBancos[]
    FC-->>CM: List<FebrabanBank>
    CM-->>BS: cached/mapped
    BS-->>BC: List<BankDto>
    BC-->>FE: ApiResponse<BankDto[]>

    alt Febraban indisponível
        FC-->>BS: Exception
        BS->>CM: GetOrCreateAsync("bcbClientCache_...")
        CM->>BCb: GetBankAsync(filter)
        BCb->>BCB: POST /pessoasJuridicas
        BCB-->>BCb: content[]
        BCb-->>CM: List<BcbBank>
        CM-->>BS: cached/mapped
    end
```

## Como Rodar

### Pré-requisitos

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Docker (opcional)

### Comandos

```bash
# Clone
https://github.com/afonsoft/bff.git
cd bff

# Restaurar
dotnet restore Eaf.Template.Bff.sln

# Build
dotnet build Eaf.Template.Bff.sln --configuration Release

# Testes
dotnet test tests/Eaf.Template.Bff.Tests/Eaf.Template.Bff.Tests.csproj

# Executar localmente
dotnet run --project src/Eaf.Template.Bff.Host
```

Acesse o Swagger em `http://localhost:4000/swagger` (ou porta configurada em `ASPNETCORE_URLS`).

### Docker

```bash
docker build -t eaf-template-bff .
docker run -d -p 5000:5000 -e DOTNET_PROCESSOR_COUNT=2 eaf-template-bff
```

### Variáveis de Ambiente Relevantes

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| `API_URL_BCB` | URL base do serviço BCB | `https://www3.bcb.gov.br/informes/rest/pessoasJuridicas` |
| `API_URL_FEBRABAN` | URL base do serviço Febraban | `https://portal.febraban.org.br/Associado/Index` |
| `ASPNETCORE_URLS` | URLs da aplicação | `http://+:4000` |
| `DOTNET_PROCESSOR_COUNT` | Núcleos de CPU | `2` |

## Testes e Cobertura

```bash
# Testes com cobertura
dotnet test tests/Eaf.Template.Bff.Tests/Eaf.Template.Bff.Tests.csproj \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults
```

| Métrica | Valor |
|---------|-------|
| Total de Testes | 37 |
| Testes Passando | 37 |
| Cobertura de Linhas | 27,91% (211/756) |
| Cobertura de Branches | 15,89% (41/258) |
| Core | 26,44% |
| Host | 7,22% |
| Proxy | 86,11% |

> A meta do projeto é atingir **90% de cobertura**. A camada `Proxy` já está bem coberta; `Core` e `Host` ainda precisam de testes adicionais para middlewares, controllers, extensões e configurações.

## Visão de Negócio

O BFF atua como uma **fachada segura e resiliente** entre o frontend e os serviços públicos do sistema financeiro nacional. Ele reduz a complexidade da integração para o cliente, esconde diferenças de contrato entre Febraban e BCB, melhora a performance via cache e garante alta disponibilidade através de fallback automático.

## Visão Técnica

- **Injeção de dependência**: Serviços e clientes são registrados via `ServiceCollectionExtensions`.
- **Cache distribuído**: `CacheManager` serializa (Newtonsoft.Json) e comprime (GZip) valores antes de armazenar em `IDistributedCache`.
- **Resiliência**: `HttpClient` do `IHttpClientFactory` com políticas do `Microsoft.Extensions.Http.Resilience`.
- **Observabilidade**: `OpenTelemetryExtensions` configura tracing, métricas e exporters OTLP/Prometheus.
- **Segurança**: JWT Bearer, CORS, headers sanitizados e middleware centralizado de exceções.
- **Documentação**: Swagger com anotações e `ApiResponse<T>` padronizado.

## Desenvolvedores / Contribuintes

Veja o histórico de contribuições em [CHANGELOG.md](./CHANGELOG.md).

## Licença

Este projeto está licenciado sob a licença MIT. Veja [LICENSE](./LICENSE).

## Status do Projeto

**Em desenvolvimento ativo.**

- Build: passando
- Testes: 37/37 passando
- Vulnerabilidades: nenhuma detectada (`dotnet list package --vulnerable`)
- Cobertura: em evolução (meta 90%)

## Links

- [CHANGELOG.md](./CHANGELOG.md)
- [Documentação em português (docs/README.md)](./docs/README.md)
- [Stack tecnológica (docs/technologies.md)](./docs/technologies.md)
- [Pacotes e dependências (docs/packages.md)](./docs/packages.md)
- [Funcionalidades (docs/features.md)](./docs/features.md)
- [Referência da API (docs/api.md)](./docs/api.md)
- [Issues](https://github.com/afonsoft/bff/issues)
- [Actions](https://github.com/afonsoft/bff/actions)
