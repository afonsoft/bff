# Features

## Bacen / Bank

- `GET /api/bacen?filter={filter}` — filtered list of banks.
- Fallback: tries Febraban first, then BCB.
- Distributed cache for 6h (sliding) / 24h (absolute).

## Health Checks

- `/health` endpoint for health verification.

## Observability

- OpenTelemetry traces/metrics.
- Serilog structured logs.
- Prometheus exporter (configurable).

## Resilience

- Retry, circuit breaker, and timeout on Bacen/BCB and Febraban clients.
