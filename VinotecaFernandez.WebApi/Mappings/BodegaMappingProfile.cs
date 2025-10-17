using AutoMapper;
using Vinoteca.Applications.Dtos.Bodega;
using Vinoteca.Entities;

namespace VinotecaFernandez.WebApi.Mappings
{
    public class BodegaMappingProfile : Profile
    {
        public BodegaMappingProfile()
        {
            CreateMap<Bodega, BodegaResponseDto>();
            CreateMap<BodegaRequestDto, Bodega>();

        }
    }
}
