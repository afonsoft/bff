# Context Engineering

## Estratégias de Carregamento

| Tipo | Quando | Exemplos |
|---|---|---|
| Always-on | Sempre carregado | `CLAUDE.md`, `.claude/rules/global-rules.md` |
| Pattern-matched | Por arquivo | `.claude/rules/dotnet.md` para `**/*.cs` |
| On-demand | Sob demanda | `.claude/skills/bff-development.md`, knowledge |
| Progressive disclosure | Codebase grande | Mapa de dirs -> headers -> conteúdo |

## Hierarquia de Prioridade
1. `CLAUDE.md` (raiz)
2. `.claude/rules/global-rules.md`
3. `.claude/rules/{dominio}.md` (por `paths:`)
4. `.claude/skills/{skill}/SKILL.md` (quando invocada)
5. `.claude/knowledge/{dominio}.md` (referenciada)

## Token Budget
- Reservar 20% do contexto para output.
- Arquivos >500 linhas: carregar headers iniciais, depois expandir trechos conforme necessário.

## Context Compaction
- Budget reduction -> snip -> microcompact -> collapse -> auto-compact.
- Preferir referenciar arquivos em vez de duplicar conteúdo.
