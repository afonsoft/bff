using Eaf.Template.Bff.Core.Models;
using Eaf.Template.Bff.Core.Services.Bacen;
using Eaf.Template.Bff.Core.Services.Bacen.Models;
using Eaf.Template.Bff.Host.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Eaf.Template.Bff.Tests.Features.Controllers;

/// <summary>
/// Testes unitários para o BacenController.
/// </summary>
public class BacenControllerTests
{
    [Fact(DisplayName = "Dado: Service retorna bancos | Quando: Get chamado | Então: Deve retornar Ok com resposta")]
    public async Task BacenController_Get_ComBancos_DeveRetornarOk()
    {
        var expected = new List<BankDto> { new() { Id = 1, Name = "BB" } };
        var mockService = new Mock<IBacenService>();
        mockService.Setup(s => s.GetBanksAsync(It.IsAny<string>())).ReturnsAsync(expected);

        var controller = new BacenController(mockService.Object);

        var result = await controller.Get("bb");

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsAssignableFrom<ApiResponse<object>>(okResult.Value);
        response.Success.Should().BeTrue();
        response.Response.Should().BeEquivalentTo(expected);
    }
}
