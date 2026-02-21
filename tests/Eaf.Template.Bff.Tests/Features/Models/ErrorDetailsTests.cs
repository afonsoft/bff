using Eaf.Template.Bff.Core.Models;

namespace Eaf.Template.Bff.Tests.Features.Models;

/// <summary>
/// Testes unitários para o modelo ErrorDetails
/// </summary>
public class ErrorDetailsTests
{
    [Fact(DisplayName = "Dado: ErrorDetails completo | Quando: Verificado | Então: Deve ter todas as propriedades")]
    public void ErrorDetails_Completo_DeveTerTodasPropriedades()
    {
        // Arrange
        var timeStamp = DateTime.UtcNow;
        var source = "TestController";
        var exceptionType = "ArgumentException";
        var message = "Test error message";
        var exception = "System.ArgumentException: Test error";
        var additionalInfo = "Additional context";

        // Act
        var errorDetails = new ErrorDetails(timeStamp, source, exceptionType, message, exception, additionalInfo);

        // Assert
        errorDetails.TimeStamp.Should().Be(timeStamp);
        errorDetails.Source.Should().Be(source);
        errorDetails.ExceptionType.Should().Be(exceptionType);
        errorDetails.Message.Should().Be(message);
        errorDetails.Exception.Should().Be(exception);
        errorDetails.AdditionalInformation.Should().Be(additionalInfo);
    }

    [Fact(DisplayName = "Dado: ErrorDetails mínimo | Quando: Verificado | Então: Deve ter propriedades obrigatórias")]
    public void ErrorDetails_Minimo_DeveTerPropriedadesObrigatorias()
    {
        // Arrange
        var timeStamp = DateTime.UtcNow;
        var source = "TestController";
        var exceptionType = "ArgumentException";
        var message = "Test error message";
        var exception = "System.ArgumentException: Test error";

        // Act
        var errorDetails = new ErrorDetails(timeStamp, source, exceptionType, message, exception);

        // Assert
        errorDetails.TimeStamp.Should().Be(timeStamp);
        errorDetails.Source.Should().Be(source);
        errorDetails.ExceptionType.Should().Be(exceptionType);
        errorDetails.Message.Should().Be(message);
        errorDetails.Exception.Should().Be(exception);
        errorDetails.AdditionalInformation.Should().BeNull();
    }

    [Fact(DisplayName = "Dado: ErrorDetails com ToString | Quando: Chamado | Então: Deve retornar JSON")]
    public void ErrorDetails_ToString_DeveRetornarFormatoJson()
    {
        // Arrange
        var errorDetails = new ErrorDetails(
            DateTime.UtcNow,
            "TestController",
            "ArgumentException",
            "Test error",
            "System.ArgumentException: Test error"
        );

        // Act
        var result = errorDetails.ToString();

        // Assert
        result.Should().Contain("TestController");
        result.Should().Contain("ArgumentException");
        result.Should().Contain("Test error");
    }
}
