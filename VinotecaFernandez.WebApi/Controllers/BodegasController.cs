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
        public async Task<IActionResult> All()
        {
            var id = User.FindFirst("Id").Value.ToString();
            var user = _userManager.FindByIdAsync(id).Result;
            if (_userManager.IsInRoleAsync(user, "Administrador").Result)
            {
                var name = User.FindFirst("name");
                var a = User.Claims;
                return Ok(_mapper.Map<IList<BodegaResponseDto>>(_bodega.GetAll()));
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
            {
                return BadRequest();
            }
            Bodega bodega = _bodega.GetById(Id.Value);
            if (bodega is null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<BodegaResponseDto>(bodega));
        }

        [HttpPost]
        public async Task<IActionResult> Crear(BodegaRequestDto bodegaRequestDto)
        {
            if (!ModelState.IsValid)
            { return BadRequest(); }
            var bodega = _mapper.Map<Bodega>(bodegaRequestDto);
            _bodega.Save(bodega);
            return Ok(bodega.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? Id, BodegaRequestDto bodegaRequestDto)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }
            var bodega = _bodega.GetById(Id.Value);
            if (bodega is null)
            { return NotFound(); }
            bodega = _mapper.Map<Bodega>(bodegaRequestDto);
            _bodega.Save(bodega);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? Id)
        {
            if (!Id.HasValue)
            { return BadRequest(); }
            if (!ModelState.IsValid)
            { return BadRequest(); }
            var bodega = _bodega.GetById(Id.Value);
            if (bodega is null)
            { return NotFound(); }
            _bodega.Delete(bodega.Id);
            return Ok();
        }
    }
}
