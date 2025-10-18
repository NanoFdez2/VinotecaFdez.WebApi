using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vinoteca.Applications;
using Vinoteca.Applications.Dtos.Provincia;
using Vinoteca.Entities;

namespace VinotecaFernandez.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciasController : ControllerBase
    {
        private readonly ILogger<ProvinciasController> _logger;
        private readonly IApplication<Provincia> _provincia;
        private readonly IMapper _mapper;
        public ProvinciasController(ILogger<ProvinciasController> logger, IApplication<Provincia> provincia, IMapper mapper)
        {
            _logger = logger;
            _provincia = provincia;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(_mapper.Map<IList<ProvinciaResponseDto>>(_provincia.GetAll()));
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest();

            Provincia provincia = _provincia.GetById(Id.Value);
            if (provincia is null)
                return NotFound();

            return Ok(provincia);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ProvinciaRequestDto provinciaRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            Provincia aux = new Provincia();
            aux = aux.devolverProvincia(provinciaRequestDto);

            Provincia provincia = _mapper.Map<Provincia>(aux);
            _provincia.Save(provincia);
            return Ok(provincia.Id);
        }
        [HttpPut]
        public async Task<IActionResult> Editar(int? Id, ProvinciaRequestDto provinciaRequestDto)
        {
            if (!Id.HasValue)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            var provincia = _provincia.GetById(Id.Value);
            if (provincia is null)
                return NotFound();

            Provincia aux = new Provincia();
            aux = aux.devolverProvincia(provinciaRequestDto);

            provincia = _mapper.Map<Provincia>(aux);
            _provincia.Save(provincia);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? Id)
        {
            if (!Id.HasValue)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            var provincia = _provincia.GetById(Id.Value);
            if (provincia is null)
                return NotFound();

            _provincia.Delete(provincia.Id);
            return Ok();
        }
    }
}
