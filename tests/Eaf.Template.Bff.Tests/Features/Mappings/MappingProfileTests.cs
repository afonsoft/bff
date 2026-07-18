using AutoMapper;
using Eaf.Template.Bff.Core.Services.Bacen.Models;
using Eaf.Template.Bff.Proxy.Bacen;
using Microsoft.Extensions.Logging.Abstractions;

namespace Eaf.Template.Bff.Tests.Features.Mappings;

/// <summary>
/// Testes unitários para o perfil de mapeamento do AutoMapper.
/// </summary>
public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance).CreateMapper();
    }

    [Fact(DisplayName = "Dado: FebrabanBank | Quando: Mapeado para BankDto | Então: Deve copiar propriedades")]
    public void MappingProfile_FebrabanBank_DeveMapearParaBankDto()
    {
        var source = new FebrabanBank
        {
            Id = 1,
            Name = "Banco do Brasil",
            Compensation = "001",
            IdBacen = "00000000"
        };

        var result = _mapper.Map<BankDto>(source);

        result.Id.Should().Be(1);
        result.Name.Should().Be("Banco do Brasil");
        result.Compensation.Should().Be("001");
        result.IdBacen.Should().Be("00000000");
    }

    [Fact(DisplayName = "Dado: BcbBank | Quando: Mapeado para BankDto | Então: Deve copiar propriedades")]
    public void MappingProfile_BcbBank_DeveMapearParaBankDto()
    {
        var source = new BcbBank
        {
            Id = 2,
            Name = "Itau",
            Compensation = "341",
            IdBacen = "60701190"
        };

        var result = _mapper.Map<BankDto>(source);

        result.Id.Should().Be(2);
        result.Name.Should().Be("Itau");
        result.Compensation.Should().Be("341");
        result.IdBacen.Should().Be("60701190");
    }
}
