using AutoMapper;
using Vinoteca.Applications;
using Vinoteca.Applications.Dtos.Vino;
using Vinoteca.Applications.Dtos.Bodega;
using Vinoteca.Applications.Dtos.Variedad;
using Vinoteca.Entities;
using Vinoteca.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace VinotecaFernandez.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class VinosController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<VinosController> _logger;
        private readonly IApplication<Vino> _vino;
        private readonly IMapper _mapper;

        public VinosController(ILogger<VinosController> logger, UserManager<User> userManager, IApplication<Vino> vino, IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _vino = vino;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        [Authorize(Roles = "Administrador, Cliente")]
        public async Task<IActionResult> All()
        {
            var id = User.FindFirst("Id").Value.ToString();
            var user = _userManager.FindByIdAsync(id).Result;
            if (await _userManager.IsInRoleAsync(user, "Administrador") ||
                await _userManager.IsInRoleAsync(user, "Cliente"))
            {
                var name = User.FindFirst("name");
                var a = User.Claims;
                return Ok(_mapper.Map<IList<VinoResponseDto>>(_vino.GetAll()));
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("ById")]
        [Authorize(Roles = "Administrador, Cliente")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest("Debe especificar un Id.");

            var idUser = User.FindFirst("Id")?.Value;
            var user = await _userManager.FindByIdAsync(idUser);

            if (await _userManager.IsInRoleAsync(user, "Administrador") ||
                await _userManager.IsInRoleAsync(user, "Cliente"))
            {
                var vino = _vino.GetById(Id.Value);

                if (vino is null)
                    return NotFound("Vino no encontrado.");

                return Ok(_mapper.Map<VinoResponseDto>(vino));
            }

            return Unauthorized();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Crear(VinoRequestDto vinoRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var vino = _mapper.Map<Vino>(vinoRequestDto);
            _vino.Save(vino);
            return Ok(vino.Id);
        }

        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int? Id, VinoRequestDto vinoRequestDto)
        {
            if (!Id.HasValue) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            Vino vinoBack = _vino.GetById(Id.Value);
            if (vinoBack is null) return NotFound();

            vinoBack = _mapper.Map<Vino>(vinoRequestDto);
            _vino.Save(vinoBack);
            return Ok(vinoBack);
        }

        [HttpDelete]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Borrar(int? Id)
        {
            if (!Id.HasValue) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            Vino vinoBack = _vino.GetById(Id.Value);
            if (vinoBack is null) return NotFound();
            _vino.Delete(vinoBack.Id);
            return Ok();
        }
    }
}
