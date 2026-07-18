# Agent Harness — EAF Template BFF

## Estrutura
```
.claude/
├── settings.json               # permissões (versionado)
├── rules/
│   ├── global-rules.md         # always-on
│   ├── dotnet.md               # path-scoped
│   └── tests.md                # path-scoped
├── agents/
│   ├── review.md
│   ├── plan.md
│   └── test.md
├── skills/
│   ├── bff-development/
│   └── bff-testing/
├── knowledge/
│   ├── architecture.md
│   └── external-apis.md
└── ...
.devin/
└── config.json                 # Devin CLI importa config do Claude
```

## Como Carregar
- Claude Code: lê `CLAUDE.md` e `.claude/` nativamente.
- Devin CLI: lê `CLAUDE.md` e `.devin/config.json` com `read_config_from.claude: true`.

## Adicionar Nova Skill
1. Criar diretório `.claude/skills/{nome}/`.
2. Adicionar `SKILL.md` com YAML frontmatter (`name`, `description`, `metadata.version`).
3. Referenciar em `CLAUDE.md` se sempre relevante.

## Tabela de Compatibilidade
| Plataforma | Suporte |
|---|---|
| Claude Code | Nativo |
| Devin CLI | Nativo (via `read_config_from`) |
| Cursor/Windsurf/Gemini/Copilot | Fora de escopo |

## Execução Local
```bash
dotnet build Eaf.Template.Bff.sln
dotnet test Eaf.Template.Bff.sln --collect:"XPlat Code Coverage"
dotnet list Eaf.Template.Bff.sln package --vulnerable
```
