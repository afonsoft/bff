# TOOLS.md

## Ferramentas Nativas
- `Read`, `Grep`, `Glob`, `Bash`, `Edit`, `Write` — operações de leitura/escrita/execução.
- `Read` para documentação e código-fonte.
- `Bash` para build/test/git.

## Categorias de Risco
| Categoria | Risco | Política |
|---|---|---|
| Read-only | Baixo | Livre |
| Write | Médio | Confirmação |
| Execute (build/test) | Alto | Sandboxed, logado |
| External | Variável | Rate-limited |

## MCPs e APIs
- Nenhum MCP específico configurado neste repo.
- APIs externas: Bacen/BCB e Febraban (mockar em testes).

## Headers Obrigatórios
- `Content-Type: application/json` para POSTs.
- Tratamento de `HttpRequestException` e timeouts via Polly.
