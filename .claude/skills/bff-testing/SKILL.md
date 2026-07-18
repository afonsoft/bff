---
name: bff-testing
description: >
  What: escrever e executar testes para o EAF Template BFF.
  When: ao adicionar/modificar código em Core, Proxy ou Host.
  Do NOT: não use para CI/CD ou documentação.
metadata:
  version: "1.0.0"
---

## Contexto
Testes em xUnit + Moq + FluentAssertions, localizados em `tests/Eaf.Template.Bff.Tests/`.

## Atuação
- Criar testes unitários em `Features/{Domínio}/`.
- Mockar `HttpClient` usando `Mock<HttpMessageHandler>`.
- Mockar `IDistributedCache` com `Mock<IDistributedCache>` retornando byte[] serializado.
- Validar controllers via factory quando necessário.

## Restrições
- Não chamar APIs externas reais em testes.
- Cobertura mínima: 80%.

## Exemplos
```csharp
[Fact(DisplayName = "Dado ... Quando ... Então ...")]
public async Task FooAsync()
{
   // arrange
   var handler = new Mock<HttpMessageHandler>();
   // ...
}
```

## Verification
- `dotnet test Eaf.Template.Bff.sln --collect:"XPlat Code Coverage"`
