using Eaf.Template.Bff.Core.Models;

namespace Eaf.Template.Bff.Tests.Features.Models;

/// <summary>
/// Testes unitários para o modelo ApiResponse
/// </summary>
public class ApiResponseTests
{
    [Fact(DisplayName = "Dado: ApiResponse criado com sucesso | Quando: Verificado | Então: Deve ter propriedades corretas")]
    public void ApiResponse_ComSucesso_DeveTerPropriedadesCorretas()
    {
        // Arrange
        var data = new { Id = 1, Name = "Test" };

        // Act
        var response = new ApiResponse<object>(true, data);

        // Assert
        response.Success.Should().BeTrue();
        response.Response.Should().Be(data);
        response.Error.Should().BeNull();
    }

    [Fact(DisplayName = "Dado: ApiResponse criado com erro | Quando: Verificado | Então: Deve ter propriedades de erro")]
    public void ApiResponse_ComErro_DeveTerPropriedadesDeErro()
    {
        // Arrange
        var errorDetails = new ErrorDetails(DateTime.UtcNow, "Test", "Exception", "Message", "Exception");

        // Act
        var response = new ApiResponse<object>(false, errorDetails);

        // Assert
        response.Success.Should().BeFalse();
        response.Response.Should().BeNull();
        response.Error.Should().Be(errorDetails);
    }

    [Fact(DisplayName = "Dado: ApiResponse vazio | Quando: Verificado | Então: Deve ter valores padrão")]
    public void ApiResponse_Vazio_DeveTerValoresPadrao()
    {
        // Arrange & Act
        var response = new ApiResponse<object>();

        // Assert
        response.Success.Should().BeFalse();
        response.Response.Should().BeNull();
        response.Error.Should().BeNull();
    }

    [Fact(DisplayName = "Dado: ResponseBase criado | Quando: Verificado | Então: Deve ter valores padrão")]
    public void ResponseBase_Vazio_DeveTerValoresPadrao()
    {
        // Arrange & Act
        var response = new ResponseBase();

        // Assert
        response.Success.Should().BeFalse();
        response.Response.Should().BeNull();
        response.Error.Should().BeNull();
    }
}
