---
paths:
  - "tests/**/*"
  - "**/*Tests.cs"
---

# Regras de Testes

- Usar padrão BDD: "Dado... Quando... Então..." em `DisplayName`.
- Mockar dependências externas (HttpClient, IDistributedCache, ILogger).
- Criar testes para caminhos felizes, erros e edge cases.
- Manter cobertura mínima de 80%; não diminuir sem justificativa.
- Executar `dotnet test` antes de commit.
- Usar `FluentAssertions` para legibilidade.
