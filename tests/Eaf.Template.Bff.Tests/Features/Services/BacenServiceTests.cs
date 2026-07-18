using AutoMapper;
using Eaf.Template.Bff.Core.Services.Bacen;
using Eaf.Template.Bff.Core.Services.Bacen.Models;
using Eaf.Template.Bff.Proxy.Bacen;
using Eaf.Template.Bff.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Eaf.Template.Bff.Tests.Features.Services;

/// <summary>
/// Testes unitários para o BacenService.
/// </summary>
public class BacenServiceTests
{
    private static IMapper CreateMapper() => new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance).CreateMapper();

    private static HttpClient CreateFebrabanClient(HttpStatusCode status, string responseBody)
    {
        var handler = new MockHttpMessageHandler((req, cancel) =>
        {
            if (req.Method == HttpMethod.Post && req.RequestUri!.ToString().Contains("Associado"))
            {
                return Task.FromResult(new HttpResponseMessage(status)
                {
                    Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
                });
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });

        return new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };
    }

    private static HttpClient CreateBcbClient(HttpStatusCode status, string responseBody)
    {
        var handler = new MockHttpMessageHandler((req, cancel) =>
        {
            if (req.Method == HttpMethod.Post && req.RequestUri!.ToString().Contains("pessoasJuridicas"))
            {
                return Task.FromResult(new HttpResponseMessage(status)
                {
                    Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
                });
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        });

        return new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };
    }

    private static IConfiguration CreateConfig()
    {
        var dic = new Dictionary<string, string?>
        {
            ["API_URL_BCB"] = "http://localhost/pessoasJuridicas",
            ["API_URL_FEBRABAN"] = "http://localhost/Associado/Index"
        };

        return new ConfigurationBuilder().AddInMemoryCollection(dic).Build();
    }

    [Fact(DisplayName = "Dado: Febraban retorna bancos | Quando: GetBanksAsync chamado | Então: Deve retornar lista mapeada")]
    public async Task GetBanksAsync_FebrabanComSucesso_DeveRetornarListaMapeada()
    {
        var febrabanResponse = JsonConvert.SerializeObject(new
        {
            listaBancos = new[] { new { id_banco = 1, banco = "Banco do Brasil", Compensacao = "001", idBacen = "00000000" } }
        });

        var febrabanClient = new FebrabanClient(CreateFebrabanClient(HttpStatusCode.OK, febrabanResponse));
        var bcbClient = new BcbClient(CreateBcbClient(HttpStatusCode.OK, "{}"));
        var service = new BacenService(bcbClient, febrabanClient, CreateMapper(), NullLogger<BacenService>.Instance, CreateConfig(), new FakeCacheManager());

        var result = await service.GetBanksAsync("");

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Banco do Brasil");
        result[0].Compensation.Should().Be("001");
        result[0].IdBacen.Should().Be("00000000");
    }

    [Fact(DisplayName = "Dado: Febraban falha e BCB retorna bancos | Quando: GetBanksAsync chamado | Então: Deve retornar lista do BCB")]
    public async Task GetBanksAsync_FebrabanFalhaBcbSucesso_DeveRetornarListaDoBcb()
    {
        var bcbResponse = JsonConvert.SerializeObject(new
        {
            content = new[] { new { id = 2, nome = "Itau", codigoCompensacao = "341", idBacen = "60701190" } }
        });

        var febrabanClient = new FebrabanClient(CreateFebrabanClient(HttpStatusCode.InternalServerError, "{}"));
        var bcbClient = new BcbClient(CreateBcbClient(HttpStatusCode.OK, bcbResponse));
        var service = new BacenService(bcbClient, febrabanClient, CreateMapper(), NullLogger<BacenService>.Instance, CreateConfig(), new FakeCacheManager());

        var result = await service.GetBanksAsync("itau");

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Itau");
    }

    [Fact(DisplayName = "Dado: Febraban e BCB falham | Quando: GetBanksAsync chamado | Então: Deve lançar AggregateException")]
    public async Task GetBanksAsync_AmbosFalham_DeveLancarAggregateException()
    {
        var febrabanClient = new FebrabanClient(CreateFebrabanClient(HttpStatusCode.InternalServerError, "{}"));
        var bcbClient = new BcbClient(CreateBcbClient(HttpStatusCode.InternalServerError, "{}"));
        var service = new BacenService(bcbClient, febrabanClient, CreateMapper(), NullLogger<BacenService>.Instance, CreateConfig(), new FakeCacheManager());

        var exception = await Assert.ThrowsAsync<AggregateException>(() => service.GetBanksAsync(""));

        exception.InnerExceptions.Should().HaveCount(2);
    }
}
