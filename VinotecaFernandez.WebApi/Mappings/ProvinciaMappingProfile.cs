using AutoMapper;
using Vinoteca.Applications.Dtos.Provincia;
using Vinoteca.Entities;

namespace VinotecaFernandez.WebApi.Mappings
{
    public class ProvinciaMappingProfile : Profile
    {
        public ProvinciaMappingProfile()
        {
            CreateMap<Provincia, ProvinciaResponseDto>();
            CreateMap<ProvinciaRequestDto, Provincia>();
        }
    }
}
