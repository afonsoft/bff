using Eaf.Template.Bff.Proxy.Bacen;
using Eaf.Template.Bff.Tests.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Eaf.Template.Bff.Tests.Features.Clients;

/// <summary>
/// Testes unitários adicionais para o BcbClient.
/// </summary>
public class BcbClientTests
{
    private static HttpClient CreateClient(HttpStatusCode status, string body)
    {
        var handler = new MockHttpMessageHandler((req, cancel) => Task.FromResult(new HttpResponseMessage(status)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        }));

        return new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };
    }

    [Fact(DisplayName = "Dado: Resposta válida do BCB | Quando: GetBankAsync chamado | Então: Deve retornar lista de bancos")]
    public async Task BcbClient_GetBankAsync_ComRespostaValida_DeveRetornarLista()
    {
        var response = JsonConvert.SerializeObject(new
        {
            content = new[] { new { id = 1, nome = "Banco do Brasil", codigoCompensacao = "001", idBacen = "00000000" } }
        });

        var client = new BcbClient(CreateClient(HttpStatusCode.OK, response)) { BaseUrl = "http://localhost/bcb" };

        var result = await client.GetBankAsync("");

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Banco do Brasil");
    }

    [Fact(DisplayName = "Dado: Resposta vazia do BCB | Quando: GetBankAsync chamado | Então: Deve retornar lista vazia")]
    public async Task BcbClient_GetBankAsync_ComRespostaVazia_DeveRetornarListaVazia()
    {
        var client = new BcbClient(CreateClient(HttpStatusCode.OK, "{}")) { BaseUrl = "http://localhost/bcb" };

        var result = await client.GetBankAsync("");

        result.Should().BeEmpty();
    }

    [Fact(DisplayName = "Dado: Erro na resposta do BCB | Quando: GetBankAsync chamado | Então: Deve lançar HttpRequestException")]
    public async Task BcbClient_GetBankAsync_ComErro_DeveLancarExcecao()
    {
        var client = new BcbClient(CreateClient(HttpStatusCode.InternalServerError, "{}")) { BaseUrl = "http://localhost/bcb" };

        await Assert.ThrowsAsync<HttpRequestException>(() => client.GetBankAsync(""));
    }
}
