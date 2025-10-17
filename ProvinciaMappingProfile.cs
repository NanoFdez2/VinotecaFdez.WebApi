VinotecaFernandez.WebApi\Mappings\ProvinciaMappingProfile.cs
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
            CreateMap<ProvinciaResponseDto, Provincia>();

            // Añadido: mapeo para las peticiones (request DTO) hacia la entidad
            CreateMap<ProvinciaRequestDto, Provincia>();

            // (Opcional) mapeo inverso si lo necesitas en algún endpoint
            CreateMap<Provincia, ProvinciaRequestDto>();
        }
    }
}