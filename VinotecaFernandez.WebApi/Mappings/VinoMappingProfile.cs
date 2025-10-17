using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vinoteca.Applications.Dtos.Vino;
using Vinoteca.Entities;

namespace VinotecaFernandez.WebApi.Mappings
{
    
    public class VinoMappingProfile : Profile
    {
        public VinoMappingProfile()
        {
            CreateMap<Vino, VinoResponseDto>();//.
               //ForMember(dest => dest.Id, ori => ori.MapFrom(src => src.Id));
            CreateMap<VinoRequestDto, Vino>();
        }
    }
}
