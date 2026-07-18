# Arquitetura do EAF Template BFF

## Visão Geral
BFF (Back-end for Front-end) em .NET 10 que expõe endpoints otimizados para frontends, consumindo APIs externas do Bacen/BCB e Febraban.

## Camadas
- **Core**: modelos, serviços, cache, configurações, middlewares, exceptions.
- **Proxy**: clientes HTTP (`BcbClient`, `FebrabanClient`).
- **Host**: API ASP.NET Core, controllers, Startup, Program, appsettings.

## Fluxo de Dados
1. Frontend -> `BacenController`
2. `IBacenService` -> `BacenService`
3. Cache (`ICacheManager`/`IDistributedCache`) -> `FebrabanClient` ou `BcbClient` (fallback)
4. Resposta `ApiResponse<T>`

## Padrões
- Clean Architecture (sem dependência circular).
- DI via interfaces.
- Circuit Breaker / Retry via `Microsoft.Extensions.Http.Resilience` + Polly.
- Caching distribuído com compressão GZip.
- Observability via OpenTelemetry/Serilog.
