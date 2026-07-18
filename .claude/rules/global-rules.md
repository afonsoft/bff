# EAF Template BFF — Global Rules

> Compatível com: Claude Code, Devin CLI

Você é um assistente de desenvolvimento do EAF Template BFF. Siga estas regras ao gerar código, revisar PRs ou responder perguntas.

---

## Escopo do Agent

O agent pode:
- Analisar código e documentação
- Propor mudanças
- Criar branches de trabalho
- Gerar commits somente em branches permitidas

O agent possui autonomia para executar ações fora destas regras desde que solicitado.

---

## Hard Rules (Bloqueio Imediato)

### Branches Protegidas
É estritamente proibido push/commit direto em:
- `main`
- `master`
- `develop`

Se solicitado, o agent deve exibir warning e pedir confirmação.

### Workflows Protegidos
É estritamente proibido modificar arquivos ou criar commits no diretório `/.github/workflows`.

### Secrets
Nunca commitar `.env`, `*.pem`, `*.key`, `secrets.*` ou credenciais.

---

## Estratégia de Branch (Obrigatória)

Toda alteração DEVE ocorrer em uma branch dedicada.

### Padrão de nomenclatura obrigatório
```
feature/{AgentLLM}-{YYYYMMDD}-{descricao-curta}
```

Exemplo válido:
```
feature/devin-20260718-update-global-rules
```

Regras:
- `YYYYMMDD` = data real da criação
- `descricao-curta` em inglês, kebab-case
- `AgentLLM` = nome do agent (devin, claude, etc.)
- Criar nova branch baseada em `main` ou `master`

---

## Planejamento Antes da Execução (MANDATÓRIO)

Antes de qualquer modificação, o agent DEVE produzir um plano explícito.

**Claude Code:** Use `/plan` antes de executar.
**Devin CLI:** Use sub-agent `.claude/agents/plan.md`.

### Formato obrigatório
```
Execution Plan:
1. Goal and context
2. Impacted files and modules
3. Implementation strategy
4. Risks and mitigations
5. Validation steps (tests, build, lint)
```

---

## Reavaliação Obrigatória

Após escrever o plano, o agent deve:
- Reavaliar riscos
- Checar conflitos com outras rules
- Confirmar aderência às convenções do projeto

---

## Stack Tecnológica

### .NET 10 / ASP.NET Core
- TargetFramework `net10.0`
- LangVersion `14.0`
- Clean Architecture: Core -> Proxy -> Host

### Testes
- xUnit 2.9.3
- Moq 4.20.72
- FluentAssertions 8.10.0
- Microsoft.AspNetCore.Mvc.Testing 10.0.10

### Observabilidade
- Serilog.AspNetCore 10.0.0
- OpenTelemetry 1.17.0

---

## Convenções do Projeto

- Idioma do código: inglês
- Idioma de documentação/testes: português (BDD Dado/Quando/Então)
- XML docs em APIs públicas
- Async/await em I/O e chamadas HTTP
- Mockar dependências externas em testes
- Cobertura mínima: 80%

---

## Skills e Knowledge

| Plataforma | Skills | Rules | Knowledge |
|---|---|---|---|
| Base | `.claude/skills/` | `.claude/rules/` | `.claude/knowledge/` |

---

## Comportamento Obrigatório

- Apresentar Execution Plan antes de qualquer modificação
- Nunca executar código sem plano aprovado
- Bloquear ações em branches protegidas
- Rejeitar workflows fora das regras
- Garantir que commits só ocorram em branches permitidas
- Priorizar segurança operacional sobre conveniência

---

## Resumo Executivo

- Planejar antes de executar
- Trabalhar apenas em branches permitidas
- Reavaliar antes de aplicar mudanças
- Nunca tocar em `main`, `master`, `develop`
- Nunca modificar `/.github/workflows` sem aprovação
