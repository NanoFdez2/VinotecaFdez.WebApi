using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vinoteca.Applications;
using Vinoteca.Applications.Dtos.Bodega;
using Vinoteca.Entities;

namespace VinotecaFernandez.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BodegasController : ControllerBase
    {
        private readonly ILogger<BodegasController> _logger;
        private readonly IApplication<Bodega> _bodega;
        private readonly IMapper _mapper;
        public BodegasController(ILogger<BodegasController> logger, IApplication<Bodega> bodega, IMapper mapper)
        {
            _logger = logger;
            _bodega = bodega;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(_mapper.Map<IList<BodegaResponseDto>>(_bodega.GetAll()));
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
