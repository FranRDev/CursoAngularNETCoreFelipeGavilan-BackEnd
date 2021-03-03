using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        [HttpGet]
        public async Task<List<GeneroDTO>> Get([FromQuery] PaginacionDTO paginacionDTO) {
            var consultable = contexto.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(consultable);
            var generos = await consultable.OrderBy(g => g.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapeador.Map<List<GeneroDTO>>(generos);
        }

        [HttpGet("ObtenerTodos")]
        public async Task<List<GeneroDTO>> ObtenerTodos() {
            return mapeador.Map<List<GeneroDTO>>(await contexto.Generos.ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneroDTO>> Get(int id) {
            var genero = await contexto.Generos.FirstOrDefaultAsync(g => g.ID == id);

            if (genero == null) { return NotFound(); }

            return mapeador.Map<GeneroDTO>(genero);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO) {
            contexto.Add(mapeador.Map<Genero>(generoCreacionDTO));
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO) {
            var genero = await contexto.Generos.FirstOrDefaultAsync(g => g.ID == id);

            if (genero == null) { return NotFound(); }

            genero = mapeador.Map(generoCreacionDTO, genero);

            await contexto.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            var existe = await contexto.Generos.AnyAsync(g => g.ID == id);

            if (!existe) { return NotFound(); }

            contexto.Remove(new Genero() { ID = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }

    }

}