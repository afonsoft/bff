# Funcionalidades

## Bacen / Banco
- `GET /api/bacen?filter={filter}` — lista de bancos filtrados.
- Fallback: tenta Febraban primeiro, depois BCB.
- Cache distribuído por 6h (sliding) / 24h (absolute).

## Health Checks
- Endpoint `/health` para verificação de saúde.

## Observabilidade
- OpenTelemetry traces/métricas.
- Serilog logs estruturados.
- Prometheus exporter (configurável).

## Resiliência
- Retry, circuit breaker e timeout nos clientes Bacen/BCB e Febraban.
