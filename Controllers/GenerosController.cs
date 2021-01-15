using back_end.Entidades;
using back_end.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace back_end.Controllers {

    [Route("api/generos")]
    public class GenerosController: ControllerBase {

        private readonly IRepositorio repositorio;

        public GenerosController(IRepositorio repositorio) {
            this.repositorio = repositorio;
        }

        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listadogeneros")]
        public List<Genero> Get() {
            return repositorio.ObtenerGeneros();
        }

        [HttpGet("{id:int}/{nombre=Roberto}")]
        public ActionResult<Genero> Get(int id, string nombre) {
            var genero = repositorio.ObtenerGeneroPorId(id);

            if (genero == null) {
                return NotFound();
            }

            return genero;
        }

        [HttpPost]
        public ActionResult Post() {
            return NoContent();
        }

        [HttpPut]
        public ActionResult Put() {
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete() {
            return NoContent();
        }

    }

}