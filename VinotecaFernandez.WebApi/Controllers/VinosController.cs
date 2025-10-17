using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vinoteca.Entities;
using Vinoteca.Services;

namespace VinotecaFernandez.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VinosController : ControllerBase
    {
        private static readonly List<Vino> vinos = new List<Vino>
        {
            new Vino { Id = 1, Nombre = "Malbec Shuano", BodegaId = 1, Anio = 2020 },
            new Vino { Id = 2, Nombre = "Cabernet Sauvignon", BodegaId = 2, Anio = 2019 },
            new Vino { Id = 3, Nombre = "Rosado Melvoú", BodegaId = 3, Anio = 2021 },
            new Vino { Id = 4, Nombre = "Chagdone Setenier", BodegaId = 4, Anio = 2016 }
        };
        private readonly ILogger<VinosController> _logger;
        private readonly IStringServices _stringServices;

        public VinosController(ILogger<VinosController> logger, IStringServices stringServices)
        {
            _logger = logger;
            _stringServices = stringServices;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> All()
        {
            return Ok(vinos);
        }

        [HttpGet]
        [Route("ById")]
        public async Task<IActionResult> ById(int? Id)
        {
            Vino vino = vinos.FirstOrDefault(l => l.Id == Id);
            if (vino is null)
            {
                return NotFound();
            }
            return Ok(vino);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Vino vino)
        {
            vino.Id = 5;
            vinos.Add(vino);
            //return Created();
            return Ok(vino);
        }

        [HttpPut]
        public async Task<IActionResult> Editar(int? Id, string nombre, int bodega)
        {
            Vino vino = vinos.FirstOrDefault(l => l.Id == Id);
            if (vino is null)
            {
                return NotFound();
            }
            vino.Nombre = nombre;
            vino.BodegaId = bodega;
            return Ok(vino);
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int? Id)
        {
            Vino vino = vinos.FirstOrDefault(l => l.Id == Id);
            if (vino is null)
            {
                return NotFound();
            }
            vinos.Remove(vino);
            return Ok();
        }
    }
}
