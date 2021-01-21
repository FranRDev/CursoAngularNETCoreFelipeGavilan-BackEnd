using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers {

    [Route("api/generos")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController : ControllerBase {

        private readonly ILogger<GenerosController> registrador;
        private readonly ApplicationDbContext contexto;
        private readonly IMapper mapeador;

        public GenerosController(ILogger<GenerosController> registrador, ApplicationDbContext contexto, IMapper mapeador) {
            this.registrador = registrador;
            this.contexto = contexto;
            this.mapeador = mapeador;
        }

        [HttpGet] // api/generos
        public async Task<List<GeneroDTO>> Get([FromQuery] PaginacionDTO paginacionDTO) {
            var consultable = contexto.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(consultable);
            var generos = await consultable.OrderBy(g => g.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapeador.Map<List<GeneroDTO>>(generos);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Genero> Get(int id) {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO) {
            contexto.Add(mapeador.Map<Genero>(generoCreacionDTO));
            await contexto.SaveChangesAsync();
            return NoContent();
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