# CLAUDE.md — EAF Template BFF

## Missão
EAF Template BFF is a .NET 10 Back-end for Front-end (BFF) template. This repo provides a dedicated backend layer that aggregates and optimizes data for frontend applications, integrating with Brazilian Central Bank (Bacen/BCB) and Febraban APIs.

## Stack Tecnológica
| Tecnologia | Versão | Uso |
|---|---|---|
| .NET | 10.0 | Runtime/SDK |
| C# | 14 | Linguagem |
| ASP.NET Core | 10.0 | API Web |
| xUnit | 2.9.3 | Testes unitários |
| Moq | 4.20.72 | Mocks |
| FluentAssertions | 8.10.0 | Asserts |
| AutoMapper | 16.2.0 | Mapeamento |
| Serilog | 10.0.0 | Logging |
| OpenTelemetry | 1.17.0 | Observability |
| Polly / Microsoft.Extensions.Http.Resilience | 10.8.0 | Resiliência |
| Swashbuckle | 10.2.3 | Swagger |
| Newtonsoft.Json | 13.0.4 | Serialização |
| FluentValidation | 12.1.1 | Validação |

## Caminhos por Plataforma
| Plataforma | Arquivo Principal | Skills | Rules | Knowledge |
|---|---|---|---|---|
| Claude Code | `CLAUDE.md` | `.claude/skills/` | `.claude/rules/` | `.claude/knowledge/` |
| Devin CLI | `CLAUDE.md` | `.claude/skills/` | `.claude/rules/` | `.claude/knowledge/` |

## Padrões de Código
- DO: seguir Clean Architecture (Core -> Proxy -> Host), injeção de dependências, async/await, logs estruturados com Serilog, XML docs em APIs públicas.
- DON'T: hardcode URLs ou secrets, chamar APIs externas em testes sem mock, expor detalhes sensíveis em responses.
- Princípios: SOLID, BFF pattern, Circuit Breaker via Polly/resilience, caching distribuído, JWT opcional.

## Hard Rules
- Nunca fazer push/commit direto em `main`, `master` ou `develop`.
- Nunca modificar arquivos em `/.github/workflows` sem aprovação humana.
- Nunca commitar `.env`, `*.pem`, `*.key`, secrets ou credenciais.
- Nunca adicionar `Newtonsoft.Json` sem justificativa (já usado no projeto).
- Só executar `git push` para branches feature/bug/hotfix.

## Soft Rules
- Alterar Dockerfile -> validar build local.
- Modificar dependências NuGet -> executar `dotnet test`.
- Deploy para produção -> requer confirmação.
- Remover testes existentes -> justificar cobertura.

## Agent Loop
Use **Plan-and-Execute**:
1. Receber tarefa.
2. Carregar `CLAUDE.md` + `.claude/rules/global-rules.md`.
3. Carregar skills/rules scopadas por caminho.
4. Apresentar `Execution Plan` e aguardar aprovação.
5. Verificar guardrails (`settings.json`/`.devin/config.json`).
6. Executar em sandbox (build/test).
7. Verification loop: `dotnet build` -> `dotnet test` -> `dotnet list package --vulnerable`.
8. Validar e ajustar (máx. 2 iterações antes de escalar).
9. Atualizar `MEMORY.md`.

## Response Style
- Idioma: português para documentação e mensagens; inglês para código/nomenclatura.
- Formato: markdown estruturado, bullets, tabelas.
- Verbosity: conciso; evidência antes de afirmação.

## Referências
- [docs/](../docs/) — Documentação do sistema
- [.claude/rules/](.claude/rules/) — Guardrails
- [.claude/skills/](.claude/skills/) — Skills especializadas
- [.claude/agents/](.claude/agents/) — Sub-agents
- [.claude/knowledge/](.claude/knowledge/) — Conhecimento de domínio
- [.devin/config.json](.devin/config.json) — Configuração Devin CLI
