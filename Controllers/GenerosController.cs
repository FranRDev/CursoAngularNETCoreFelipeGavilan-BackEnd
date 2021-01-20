using back_end.Entidades;
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

        private readonly ILogger<GenerosController> logger;

        public GenerosController(ILogger<GenerosController> logger) {
            this.logger = logger;
        }

        [HttpGet]
        public List<Genero> Get() {
            return new List<Genero>() { new Genero() { ID = 1, Nombre = "Comedia" } };
        }

        [HttpGet("{id:int}")]
        public ActionResult<Genero> Get(int id) {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Genero genero) {
            throw new NotImplementedException();
        }

        [HttpPut]
        public ActionResult Put([FromBody] Genero genero) {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public ActionResult Delete() {
            throw new NotImplementedException();
        }

    }

}