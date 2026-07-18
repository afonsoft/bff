# RULES.md

## Hard Rules (bloqueio imediato)
- Push para `main`, `master`, `develop` é proibido.
- Modificar `/.github/workflows` sem aprovação é proibido.
- Secrets/credenciais nunca são commitados.
- Código que quebra `dotnet build` ou `dotnet test` não pode ser commitado.

## Soft Rules (warning + confirmação)
- Alterar Dockerfile requer build local.
- Modificar dependências requer `dotnet list package --vulnerable`.
- Deploy para produção requer confirmação.

## Permissões por Ambiente
- Local: build/test livre.
- CI: build, test, security scan.
- Produção: deploy manual, tag.

## Tool Permissions
- Read-only por padrão.
- Write/Edit via aprovação do agente pai.
- Bash(git push:*) bloqueado para branches protegidas.
