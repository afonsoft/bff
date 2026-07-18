# APIs Externas

## Bacen / BCB
- URL base: `https://www3.bcb.gov.br/informes/rest/pessoasJuridicas`
- Endpoint: POST `/` com body `{ nome, segmento, ... }`
- Retorno: `BcbResponse` com `content` (lista de `BcbBank`)

## Febraban
- URL base: `https://portal.febraban.org.br/Associado/Index`
- Endpoint: POST `/` com body `{ FiltroAssociado, Busca }`
- Retorno: `BankResponse` com `listaBancos`

## Configuração
- `API_URL_BCB` e `API_URL_FEBRABAN` sobrescrevem as URLs padrão.
- Em testes, usar `Mock<HttpMessageHandler>` para simular responses.
