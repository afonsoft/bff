# API Documentation

## Endpoints

### GET /api/bacen

Lists banks using an optional filter.

**Query params:**

- `filter` (string, optional)

**Response 200:**

```json
{
  "success": true,
  "response": [
    {
      "id": 1,
      "name": "Banco do Brasil S.A.",
      "compensation": "001",
      "idBacen": "00000000"
    }
  ]
}
```

**Response 500:**

```json
{
  "success": false,
  "error": {
    "message": "...",
    "source": "BacenService",
    "exceptionType": "AggregateException"
  }
}
```

## Configuration

- `API_URL_BCB` — BCB base URL (default: `https://www3.bcb.gov.br/informes/rest/pessoasJuridicas/`).
- `API_URL_FEBRABAN` — Febraban base URL (default: `https://portal.febraban.org.br/Associado/`).
