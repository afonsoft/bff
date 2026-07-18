namespace Eaf.Template.Bff.Tests.Features.Extensions;

/// <summary>
/// Testes unitários para as extensões de Exception.
/// </summary>
public class ExceptionExtensionsTests
{
    [Fact(DisplayName = "Dado: Exception com mensagem | Quando: FormatException chamado | Então: Deve conter mensagem")]
    public void ExceptionExtensions_FormatException_DeveConterMensagem()
    {
        var exception = new InvalidOperationException("erro de teste");

        var result = ExceptionExtension.FormatException(exception);

        result.Should().Contain("erro de teste");
    }

    [Fact(DisplayName = "Dado: String com sufixo | Quando: RemovePostFix chamado | Então: Deve remover sufixo")]
    public void ExceptionExtensions_RemovePostFix_DeveRemoverSufixo()
    {
        var result = "Eaf.Template.Bff.Host".RemovePostFix("Host");

        result.Should().Be("Eaf.Template.Bff.");
    }

    [Fact(DisplayName = "Dado: String nula | Quando: RemovePostFix chamado | Então: Deve retornar nulo")]
    public void ExceptionExtensions_RemovePostFix_ComStringNula_DeveRetornarNulo()
    {
        string? value = null;

        var result = value.RemovePostFix("Host");

        result.Should().BeNull();
    }

    [Fact(DisplayName = "Dado: String e índice válido | Quando: Left chamado | Então: Deve retornar substring")]
    public void ExceptionExtensions_Left_DeveRetornarSubstring()
    {
        var result = "abcdef".Left(3);

        result.Should().Be("abc");
    }

    [Fact(DisplayName = "Dado: Índice maior que tamanho | Quando: Left chamado | Então: Deve lançar ArgumentException")]
    public void ExceptionExtensions_Left_ComIndiceMaior_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => "abc".Left(10));
    }
}
