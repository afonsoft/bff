using Eaf.Template.Bff.Proxy.Bacen;
using Eaf.Template.Bff.Tests.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Eaf.Template.Bff.Tests.Features.Clients;

/// <summary>
/// Testes unitários para o FebrabanClient.
/// </summary>
public class FebrabanClientTests
{
    private static HttpClient CreateClient(HttpStatusCode status, string body)
    {
        var handler = new MockHttpMessageHandler((req, cancel) => Task.FromResult(new HttpResponseMessage(status)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        }));

        return new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };
    }

    [Fact(DisplayName = "Dado: HttpClient nulo | Quando: FebrabanClient criado | Então: Deve lançar ArgumentNullException")]
    public void FebrabanClient_ComHttpClientNulo_DeveLancarExcecao()
    {
        HttpClient httpClient = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => new FebrabanClient(httpClient));

        exception.ParamName.Should().Be("httpClient");
    }

    [Fact(DisplayName = "Dado: Url sem barra | Quando: BaseUrl atribuído | Então: Deve adicionar barra")]
    public void FebrabanClient_BaseUrl_DeveAdicionarBarra()
    {
        var client = new FebrabanClient(new HttpClient());

        client.BaseUrl = "http://api.test.com/associado";

        client.BaseUrl.Should().Be("http://api.test.com/associado/");
    }

    [Fact(DisplayName = "Dado: BaseUrl com barra | Quando: BaseUrl atribuído | Então: Não deve duplicar barra")]
    public void FebrabanClient_BaseUrl_ComBarra_DeveManterUmaBarra()
    {
        var client = new FebrabanClient(new HttpClient());

        client.BaseUrl = "http://api.test.com/associado/";

        client.BaseUrl.Should().Be("http://api.test.com/associado/");
    }

    [Fact(DisplayName = "Dado: Resposta válida da Febraban | Quando: GetBankAsync chamado | Então: Deve retornar lista de bancos")]
    public async Task FebrabanClient_GetBankAsync_ComRespostaValida_DeveRetornarLista()
    {
        var response = JsonConvert.SerializeObject(new
        {
            listaBancos = new[] { new { id_banco = 1, banco = "Banco do Brasil", Compensacao = "001", idBacen = "00000000" } }
        });

        var client = new FebrabanClient(CreateClient(HttpStatusCode.OK, response)) { BaseUrl = "http://localhost/associado" };

        var result = await client.GetBankAsync("");

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Banco do Brasil");
    }

    [Fact(DisplayName = "Dado: Resposta vazia da Febraban | Quando: GetBankAsync chamado | Então: Deve retornar lista vazia")]
    public async Task FebrabanClient_GetBankAsync_ComRespostaVazia_DeveRetornarListaVazia()
    {
        var client = new FebrabanClient(CreateClient(HttpStatusCode.OK, "{}")) { BaseUrl = "http://localhost/associado" };

        var result = await client.GetBankAsync("");

        result.Should().BeEmpty();
    }

    [Fact(DisplayName = "Dado: Erro na resposta da Febraban | Quando: GetBankAsync chamado | Então: Deve lançar HttpRequestException")]
    public async Task FebrabanClient_GetBankAsync_ComErro_DeveLancarExcecao()
    {
        var client = new FebrabanClient(CreateClient(HttpStatusCode.InternalServerError, "{}")) { BaseUrl = "http://localhost/associado" };

        await Assert.ThrowsAsync<HttpRequestException>(() => client.GetBankAsync(""));
    }
}
