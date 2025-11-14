using AutoMapper;
using Vinoteca.Applications.Dtos.Identity.Role;
using Vinoteca.Entities.MicrosoftIdentity;

namespace VinotecaFernandez.WebApi.Mappings
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleResponseDto>();
            CreateMap<RoleRequestDto, Role>();
        }
    }
}
