using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vinoteca.Applications;
using Vinoteca.Applications.Dtos.Provincia;
using Vinoteca.Entities;
using Vinoteca.Entities.MicrosoftIdentity;

namespace VinotecaFernandez.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciasController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ProvinciasController> _logger;
        private readonly IApplication<Provincia> _provincia;
        private readonly IMapper _mapper;
        public ProvinciasController(ILogger<ProvinciasController> logger, UserManager<User> userManager,
            IApplication<Provincia> provincia, IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _provincia = provincia;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        [Authorize(Roles = "Administrador, Cliente")]
        public async Task<IActionResult> All()
        {
            try
            {
                var id = User.FindFirst("Id").Value.ToString();
                var user = _userManager.FindByIdAsync(id).Result;
                if (await _userManager.IsInRoleAsync(user, "Administrador") ||
                    await _userManager.IsInRoleAsync(user, "Cliente"))
                {
                    var name = User.FindFirst("name");
                    var a = User.Claims;
                    return Ok(_mapper.Map<IList<ProvinciaResponseDto>>(_provincia.GetAll()));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las provincias.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
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
                var provincia = _provincia.GetById(Id.Value);
                if (provincia is null)
                    return NotFound("Provincia no encontrada.");

                return Ok(_mapper.Map<ProvinciaResponseDto>(provincia));
            }

            return Unauthorized();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]

        public async Task<IActionResult> Crear(ProvinciaRequestDto provinciaRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            Provincia provincia = _mapper.Map<Provincia>(provinciaRequestDto);
            _provincia.Save(provincia);
            return Ok(provincia.Id);
        }


        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int? Id, ProvinciaRequestDto provinciaRequestDto)
        {
            if (!Id.HasValue)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            Provincia provinciaBack = _provincia.GetById(Id.Value);
            if (provinciaBack is null)
                return NotFound();

            provinciaBack = _mapper.Map<Provincia>(provinciaRequestDto);
            _provincia.Save(provinciaBack);
            return Ok(provinciaBack);
        }

        [HttpDelete]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Borrar(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            Provincia provinciaBack = _provincia.GetById(Id.Value);
            if (provinciaBack is null)
                return NotFound();

            _provincia.Delete(provinciaBack.Id);
            return Ok();
        }
    }
}
