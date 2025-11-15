using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vinoteca.Applications;
using Vinoteca.Applications.Dtos.Bodega;
using Vinoteca.Entities;
using Vinoteca.Entities.MicrosoftIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace VinotecaFernandez.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BodegasController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<BodegasController> _logger;
        private readonly IApplication<Bodega> _bodega;
        private readonly IMapper _mapper;
        public BodegasController(ILogger<BodegasController> logger,
            UserManager<User> userManager, IApplication<Bodega> bodega, IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _bodega = bodega;
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
                    return Ok(_mapper.Map<IList<BodegaResponseDto>>(_bodega.GetAll()));
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las bodegas.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
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
                    var bodega = _bodega.GetById(Id.Value);

                    if (bodega is null)
                        return NotFound("Bodega no encontrada.");

                    return Ok(_mapper.Map<BodegaResponseDto>(bodega));
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la bodega por Id.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Crear(BodegaRequestDto bodegaRequestDto)
        {
            try
            {
                if (!ModelState.IsValid)
                { return BadRequest(); }
                var bodega = _mapper.Map<Bodega>(bodegaRequestDto);
                _bodega.Save(bodega);
                return Ok(bodega.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la bodega.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int? Id, BodegaRequestDto bodegaRequestDto)
        {
            try
            {
                if (!Id.HasValue)
                { return BadRequest(); }
                if (!ModelState.IsValid)
                { return BadRequest(); }
                var bodegaBack = _bodega.GetById(Id.Value);
                if (bodegaBack is null)
                { return NotFound(); }
                bodegaBack = _mapper.Map<Bodega>(bodegaRequestDto);
                _bodega.Save(bodegaBack);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar la bodega.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Borrar(int? Id)
        {
            try
            {
                if (!Id.HasValue)
                { return BadRequest(); }
                if (!ModelState.IsValid)
                { return BadRequest(); }
                var bodegaBack = _bodega.GetById(Id.Value);
                if (bodegaBack is null)
                { return NotFound(); }
                _bodega.Delete(bodegaBack.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar la bodega.");
                return StatusCode(500, "Ocurrió un error al procesar la solicitud.");
            }
        }
    }
}
