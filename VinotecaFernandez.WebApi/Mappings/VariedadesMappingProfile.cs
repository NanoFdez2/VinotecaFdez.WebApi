using AutoMapper;
using Vinoteca.Applications.Dtos.Variedad;
using Vinoteca.Entities;

namespace VinotecaFernandez.WebApi.Mappings
{
    public class VariedadMappingProfile : Profile
    {
        public VariedadMappingProfile()
        {
            CreateMap<Variedad, VariedadResponseDto>();
            CreateMap<VariedadRequestDto, Variedad>();
        }
    }
}
