# Documentação da API

## Endpoints

### GET /api/bacen
Lista bancos a partir do filtro opcional.

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

## Configuração
- `API_URL_BCB` — URL base do BCB (default: `https://www3.bcb.gov.br/informes/rest/pessoasJuridicas/`).
- `API_URL_FEBRABAN` — URL base da Febraban (default: `https://portal.febraban.org.br/Associado/`).
