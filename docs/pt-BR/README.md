# Documentação do Sistema — EAF Template BFF

> **Português (pt-BR)**. A versão em inglês (en-US), padrão do repositório, está em [README.md](../README.md).

## Visão Geral

Template de Back-end for Front-end (BFF) em .NET 10, com Clean Architecture, observabilidade, resiliência e testes automatizados.

## Arquitetura

```
Frontend -> Host (Controllers) -> Core (Services) -> Proxy (HttpClients) -> APIs externas
```

## Estrutura de Diretórios

- `src/Eaf.Template.Bff.Core` — lógica de domínio, serviços, cache, middlewares e extensões.
- `src/Eaf.Template.Bff.Proxy` — clientes HTTP para Bacen/BCB e Febraban.
- `src/Eaf.Template.Bff.Host` — API ASP.NET Core.
- `tests/Eaf.Template.Bff.Tests` — testes xUnit/Moq/FluentAssertions/Shouldly/NSubstitute.
- `.github/workflows` — CI/CD, qualidade e segurança.
- `.claude/` e `.devin/` — harness e regras para agentes.
- `CLAUDE.md` — índice do harness.

## Início Rápido

```bash
dotnet restore Eaf.Template.Bff.sln
dotnet build Eaf.Template.Bff.sln
dotnet test tests/Eaf.Template.Bff.Tests/Eaf.Template.Bff.Tests.csproj
dotnet run --project src/Eaf.Template.Bff.Host
```

## Testes e Cobertura

- Total de testes: 37
- Cobertura de linhas: 27,91% (meta: 90%)
- Cobertura de branches: 15,89%
- `Proxy` já está com 86,11% de cobertura; `Core` e `Host` precisam de mais testes.

## Referências

- [README.md](../README.md) (en-US)
- [technologies.md](./technologies.md)
- [packages.md](./packages.md)
- [plugins.md](./plugins.md)
- [features.md](./features.md)
- [api.md](./api.md)
