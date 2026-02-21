using Eaf.Template.Bff.Proxy.Bacen;
using System.Net.Http;

namespace Eaf.Template.Bff.Tests.Features.HttpClient;

/// <summary>
/// Testes unitários simplificados para o BcbClient
/// </summary>
public class BcbClientSimpleTests
{
    [Fact(DisplayName = "Dado: HttpClient válido | Quando: BcbClient criado | Então: Deve inicializar corretamente")]
    public void BcbClient_ComHttpClientValido_DeveInicializarCorretamente()
    {
        // Arrange
        var httpClient = new System.Net.Http.HttpClient();

        // Act
        var client = new BcbClient(httpClient);

        // Assert
        client.Should().NotBeNull();
        client.BaseUrl.Should().Be("https://www3.bcb.gov.br/informes/rest/pessoasJuridicas/");
    }

    [Fact(DisplayName = "Dado: HttpClient nulo | Quando: BcbClient criado | Então: Deve lançar exceção")]
    public void BcbClient_ComHttpClientNulo_DeveLancarExcecao()
    {
        // Arrange
        System.Net.Http.HttpClient httpClient = null!;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new BcbClient(httpClient));
        exception.ParamName.Should().Be("httpClient");
    }

    [Fact(DisplayName = "Dado: BaseUrl personalizado | Quando: Definido | Então: Deve atualizar com barra")]
    public void BaseUrl_ComUrlPersonalizado_DeveAtualizarComBarra()
    {
        // Arrange
        var httpClient = new System.Net.Http.HttpClient();
        var client = new BcbClient(httpClient);

        // Act
        client.BaseUrl = "https://api.test.com";

        // Assert
        client.BaseUrl.Should().Be("https://api.test.com/");
    }

    [Fact(DisplayName = "Dado: BaseUrl com barra | Quando: Definido | Então: Não deve duplicar barra")]
    public void BaseUrl_ComBarra_DeveNaoDuplicarBarra()
    {
        // Arrange
        var httpClient = new System.Net.Http.HttpClient();
        var client = new BcbClient(httpClient);

        // Act
        client.BaseUrl = "https://api.test.com/";

        // Assert
        client.BaseUrl.Should().Be("https://api.test.com/");
    }
}
