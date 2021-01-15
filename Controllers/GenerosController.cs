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
        public List<Genero> Get() {
            return repositorio.ObtenerGeneros();
        }

        [HttpGet]
        public ActionResult<Genero> Get(int id) {
            var genero = repositorio.ObtenerGeneroPorId(id);

            if (genero == null) {
                return NotFound();
            }

            return genero;
        }

        [HttpPost]
        public void Post() {

        }

        [HttpPut]
        public void Put() {

        }

        [HttpDelete]
        public void Delete() {

        }

    }

}