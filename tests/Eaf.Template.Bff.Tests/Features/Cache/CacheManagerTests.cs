using Eaf.Template.Bff.Core.Models;
using Eaf.Template.Bff.Core.Services.Bacen.Models;
using Eaf.Template.Bff.Tests.Helpers;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Eaf.Template.Bff.Tests.Features.Cache;

/// <summary>
/// Testes unitários para o CacheManager.
/// </summary>
public class CacheManagerTests
{
    [Fact(DisplayName = "Dado: Cache vazio | Quando: GetAsync chamado | Então: Deve retornar nulo")]
    public async Task CacheManager_GetAsync_CacheVazio_DeveRetornarNulo()
    {
        var cache = new CacheManager(new FakeDistributedCache());

        var result = await cache.GetAsync<object>("chave");

        result.Should().BeNull();
    }

    [Fact(DisplayName = "Dado: Valor armazenado | Quando: SetAsync e GetAsync chamados | Então: Deve retornar o mesmo valor")]
    public async Task CacheManager_SetGet_DeveRetornarValor()
    {
        var cache = new CacheManager(new FakeDistributedCache());
        var expected = new ApiResponse<object>(true, new { Id = 1 });

        await cache.SetAsync("chave", expected);
        var result = await cache.GetAsync<ApiResponse<object>>("chave");

        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
    }

    [Fact(DisplayName = "Dado: Cache vazio | Quando: GetOrCreateAsync chamado | Então: Deve executar factory e armazenar")]
    public async Task CacheManager_GetOrCreateAsync_DeveExecutarFactory()
    {
        var cache = new CacheManager(new FakeDistributedCache());
        var factoryCalls = 0;

        var result1 = await cache.GetOrCreateAsync("chave", async () =>
        {
            factoryCalls++;
            return await Task.FromResult("valor");
        });

        var result2 = await cache.GetOrCreateAsync("chave", async () =>
        {
            factoryCalls++;
            return await Task.FromResult("outro");
        });

        result1.Should().Be("valor");
        result2.Should().Be("valor");
        factoryCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Dado: Valor armazenado | Quando: TryGetValue chamado | Então: Deve retornar verdadeiro")]
    public async Task CacheManager_TryGetValue_ComValor_DeveRetornarVerdadeiro()
    {
        var cache = new CacheManager(new FakeDistributedCache());
        await cache.SetAsync("chave", new BankDto { Id = 1, Name = "BB" });

        var found = cache.TryGetValue<BankDto>("chave", out var value);

        found.Should().BeTrue();
        value.Should().NotBeNull();
        value!.Name.Should().Be("BB");
    }

    [Fact(DisplayName = "Dado: Cache vazio | Quando: TryGetValue chamado | Então: Deve retornar falso")]
    public void CacheManager_TryGetValue_CacheVazio_DeveRetornarFalso()
    {
        var cache = new CacheManager(new FakeDistributedCache());

        var found = cache.TryGetValue<BankDto>("inexistente", out var value);

        found.Should().BeFalse();
        value.Should().BeNull();
    }
}
