# Documentação do Sistema — EAF Template BFF

## Visão Geral
Template de Back-end for Front-end (BFF) em .NET 10, com Clean Architecture, observabilidade, resiliência e testes automatizados.

## Arquitetura
```
Frontend -> Host (Controllers) -> Core (Services) -> Proxy (HttpClients) -> APIs externas
```

## Estrutura de Diretórios
- `src/Eaf.Template.Bff.Core` — lógica de domínio, serviços, cache, middlewares.
- `src/Eaf.Template.Bff.Proxy` — clientes HTTP para Bacen/BCB e Febraban.
- `src/Eaf.Template.Bff.Host` — API ASP.NET Core.
- `tests/Eaf.Template.Bff.Tests` — testes xUnit/Moq/FluentAssertions.
- `.github/workflows` — CI/CD, qualidade e segurança.

## Início Rápido
```bash
dotnet restore
dotnet build Eaf.Template.Bff.sln
dotnet test Eaf.Template.Bff.sln
dotnet run --project src/Eaf.Template.Bff.Host
```

## Referências
- [technologies.md](./technologies.md)
- [packages.md](./packages.md)
- [plugins.md](./plugins.md)
- [features.md](./features.md)
- [api.md](./api.md)
