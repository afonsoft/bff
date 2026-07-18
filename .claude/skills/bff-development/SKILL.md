---
name: bff-development
description: >
  What: desenvolver features no EAF Template BFF seguindo Clean Architecture e .NET 10.
  When: ao criar/modificar controllers, services, clients, configuração ou middleware.
  Do NOT: não use para CI/CD, documentação isolada ou testes (use bff-testing).
metadata:
  version: "1.0.0"
---

## Contexto
Projeto BFF em .NET 10 com três camadas:
- `Eaf.Template.Bff.Core` — domínio, services, cache, configurações, middlewares.
- `Eaf.Template.Bff.Proxy` — clientes HTTP para APIs externas (Bacen/BCB, Febraban).
- `Eaf.Template.Bff.Host` — API ASP.NET Core, controllers, Startup, Program.

## Atuação
- Adicionar XML docs em APIs públicas.
- Registrar serviços em `Startup.ConfigureServices` usando abstrações.
- Configurar resiliência/Polly em `HttpClientWithPollyExtensions`.
- Adicionar mappings em `Middlewares/MappingProfile.cs`.
- Usar `IConfiguration` para URLs (`API_URL_BCB`, `API_URL_FEBRABAN`).

## Restrições
- Não hardcodear URLs de API externa.
- Não expor detalhes de exceção em produção.
- Não quebrar contratos JSON existentes sem versionamento.

## Exemplos
```csharp
// Registrar novo client
services.AddHttpClient<NewClient>()
        .AddStandardResilienceHandler(...);
```

## Verification
- `dotnet build Eaf.Template.Bff.sln`
- `dotnet test Eaf.Template.Bff.sln`
