using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vinoteca.Applications;
using Vinoteca.Applications.Dtos.Variedad;
using Vinoteca.Entities;
using Vinoteca.Entities.MicrosoftIdentity;

namespace VinotecaFernandez.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class VariedadesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<VariedadesController> _logger;
        private readonly IApplication<Variedad> _variedad;
        private readonly IMapper _mapper;

        public VariedadesController(ILogger<VariedadesController> logger, UserManager<User> userManager
            , IApplication<Variedad> variedad, IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _variedad = variedad;
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
                    return Ok(_mapper.Map<IList<VariedadResponseDto>>(_variedad.GetAll()));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las variedades.");
                return StatusCode(500, "Ocurrió un error al solicitar.");
            }
        }

        [HttpGet]
        [Route("ById")]
        [Authorize(Roles = "Administrador, Cliente")]
        public async Task<IActionResult> ById(int? Id)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest("Debe especificar un Id.");

                var idUser = User.FindFirst("Id")?.Value;
                var user = await _userManager.FindByIdAsync(idUser);

                if (await _userManager.IsInRoleAsync(user, "Administrador") ||
                    await _userManager.IsInRoleAsync(user, "Cliente"))
                {
                    var genero = _variedad.GetById(Id.Value);

                    if (genero is null)
                        return NotFound("Variedad no encontrada.");

                    return Ok(_mapper.Map<VariedadResponseDto>(genero));
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la variedad por Id.");
                return StatusCode(500, "Ocurrió un error al solicitar.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Crear(VariedadRequestDto variedadRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var variedad = _mapper.Map<Variedad>(variedadRequestDto);
                _variedad.Save(variedad);
                return Ok(variedad.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la variedad.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int? Id, VariedadRequestDto variedadRequestDto)
        {
            try
            {
                if (!Id.HasValue)
                    return BadRequest();
                if (!ModelState.IsValid)
                    return BadRequest();

                var variedad = _variedad.GetById(Id.Value);
                if (variedad is null)
                    return NotFound();

                variedad = _mapper.Map<Variedad>(variedadRequestDto);
                _variedad.Save(variedad);
                return Ok(variedad.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar la variedad.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Borrar(int? Id)
        {
            try
            {
                if (!Id.HasValue) return BadRequest();
                if (!ModelState.IsValid) return BadRequest();

                Variedad variedadBack = _variedad.GetById(Id.Value);
                if (variedadBack is null) return NotFound();
                _variedad.Delete(variedadBack.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar la variedad.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }
    }
}
