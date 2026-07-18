# WORKFLOWS.md

## CI/CD

### `ci-build-test.yml`
- Trigger: push em `feature/*`, `bug/*`, `hotfix/*`; PR para `main`.
- Jobs: restore, build, test, upload artefatos.

### `code-quality.yml`
- Trigger: PR/push para `main`, `releases/*`, manual.
- Jobs: Qodana, SonarQube, Snyk, quality metrics.

### `security-scan.yml`
- Trigger: push/PR `main`, semanal.
- Jobs: CodeQL, Snyk, SonarQube, security summary.

### `deploy-publish.yml`
- Trigger: tag `v*`, push `main`, manual.
- Jobs: validação, build, Docker multi-plataforma, release.

## Verification Loop
Agent Output -> `dotnet build` -> `dotnet test` -> `dotnet list package --vulnerable` -> PR -> CI.

## Rollback
- Reverter commit na branch feature.
- Para releases, usar tag anterior e re-deploy da imagem Docker.
