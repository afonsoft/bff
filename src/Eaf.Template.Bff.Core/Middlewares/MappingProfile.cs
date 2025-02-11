using Eaf.Template.Bff.Core.Services.Bacen.Models;
using Eaf.Template.Bff.Proxy.Bacen;

namespace AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Create a Profile to AutoMapper or/and use Attribute Mapping
            //https://docs.automapper.org/en/stable/Attribute-mapping.html

            CreateMap<FebrabanBank, BankDto>();
            CreateMap<BcbBank, BankDto>();
        }
    }
}