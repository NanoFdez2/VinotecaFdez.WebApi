using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vinoteca.Applications;
using Vinoteca.Applications.Dtos.Variedad;
using Vinoteca.Entities;

namespace VinotecaFernandez.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariedadesController : ControllerBase
    {
        private readonly ILogger<VariedadesController> _logger;
        private readonly IApplication<Variedad> _variedad;
        private readonly IMapper _mapper;

        public VariedadesController(ILogger<VariedadesController> logger, IApplication<Variedad> variedad, IMapper mapper)
        {
            _logger = logger;
            _variedad = variedad;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(_mapper.Map<IList<VariedadResponseDto>>(_variedad.GetAll()));
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest();

            Variedad variedad = _variedad.GetById(Id.Value);
            if (variedad is null)
                return NotFound();

            return Ok(_mapper.Map<VariedadResponseDto>(variedad));
        }

        [HttpPost]
        public async Task<IActionResult> Crear(VariedadRequestDto variedadRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var variedad = _mapper.Map<Variedad>(variedadRequestDto);
            _variedad.Save(variedad);
            return Ok(variedad.Id);
        }
        [HttpPut]
        public async Task<IActionResult> Editar(int? Id, VariedadRequestDto variedadRequestDto)
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
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Borrar(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            var variedad = _variedad.GetById(Id.Value);
            if (variedad is null)
                return NotFound();

            _variedad.Delete(variedad.Id);
            return Ok();
        }
    }
}
