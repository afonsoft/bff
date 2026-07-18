# MEMORY.md

## Decisões Técnicas
| Data | Decisão | Motivo | Alternativas Descartadas |
|---|---|---|---|
| 2026-07-18 | Adotar CLAUDE.md como SSoT | Compatível com Claude Code e Devin CLI | AGENTS.md separado (duplicação) |

## Débitos Técnicos
| Item | Impacto | Prioridade |
|---|---|---|
| Cobertura de testes baixa | Risco de regressão | Alta |
| Workflows copiados do metar-decoder | Referências incorretas | Alta |

## Lições Aprendidas
| Contexto | Erro | Como Evitar |
|---|---|---|
| Atualização de pacotes | Vulnerabilidades em AutoMapper/OpenTelemetry | Verificar `dotnet list package --vulnerable` após atualização |

## Políticas de Limpeza
- Descartar memórias de branches deletadas.
- Remover fatos desatualizados e revisar a cada 3 meses.
