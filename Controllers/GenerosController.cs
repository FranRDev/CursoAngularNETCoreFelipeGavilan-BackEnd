using back_end.Entidades;
using back_end.Filtros;
using back_end.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Controllers {

    [Route("api/generos")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController : ControllerBase {

        private readonly IRepositorio repositorio;
        private readonly WeatherForecastController weatherForecastController;
        private readonly ILogger<GenerosController> logger;

        public GenerosController(IRepositorio repositorio, WeatherForecastController weatherForecastController, ILogger<GenerosController> logger) {
            this.repositorio = repositorio;
            this.weatherForecastController = weatherForecastController;
            this.logger = logger;
        }

        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listadogeneros")]
        //[ResponseCache(Duration = 60)]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public List<Genero> Get() {
            logger.LogInformation("Vamos a mostrar los géneros");
            return repositorio.ObtenerGeneros();
        }

        [HttpGet("{id:int}/")]
        public async Task<ActionResult<Genero>> Get(int id, [FromHeader] string nombre) {
            logger.LogDebug($"Obteniendo un género por el ID {id}");

            var genero = await repositorio.ObtenerGeneroPorId(id);

            if (genero == null) {
                throw new ApplicationException($"El género de ID {id} no fue encontrado.");
                logger.LogWarning($"No pudimos encontrar el género con ID {id}");
                return NotFound();
            }

            return genero;
        }

        [HttpGet("guid")]
        public ActionResult<Guid> GetGuid() {
            return Ok(new {
                GuidGeneros = repositorio.ObtenerGuid(),
                GuidWF = weatherForecastController.ObtenerGuidWC()
            });
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genero genero) {
            repositorio.CrearGenero(genero);
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genero genero) {
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete() {
            return NoContent();
        }

    }

}