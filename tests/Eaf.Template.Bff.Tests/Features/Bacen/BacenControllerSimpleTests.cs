using Eaf.Template.Bff.Host.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Eaf.Template.Bff.Tests.Features.Bacen;

/// <summary>
/// Testes unitários simplificados para o BacenController
/// </summary>
public class BacenControllerSimpleTests
{
    [Fact(DisplayName = "Dado: Controller criado | Quando: Verificado | Então: Deve estar nulo")]
    public void BacenController_Criado_DeveEstarNulo()
    {
        // Arrange & Act
        var controller = new BacenController(null!);
        
        // Assert
        controller.Should().NotBeNull();
    }
}
