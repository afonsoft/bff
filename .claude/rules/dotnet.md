---
paths:
  - "**/*.cs"
  - "**/*.csproj"
---

# Regras .NET/C#

- Preferir `async/await` e métodos com sufixo `Async`.
- Utilizar nullable reference types corretamente; anotar retornos `?` quando aplicável.
- Manter XML docs em APIs públicas.
- Não usar `ConfigureAwait(false)` em ASP.NET Core.
- Seguir PascalCase para classes/métodos/propriedades, camelCase para parâmetros.
- `LangVersion` 14.0 e `TargetFramework` net10.0.
- Adicionar `using` necessários no topo; não importar dentro de métodos.
- Preferir `IHttpClientFactory` e `HttpClient` nomeados sobre instâncias manuais.
- Não hardcodear URLs de API externa; usar `IConfiguration`.
- Sempre executar `dotnet build` e `dotnet test` após alterações.
