using AutoMapper;
using back_end.DTOs;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Controllers {

    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase {

        private const string CONTENEDOR = "actores";

        private readonly IAlmacenadorArchivos almacenador;
        private readonly IMapper mapeador;
        private readonly ApplicationDbContext contexto;

        public ActoresController(ApplicationDbContext contexto, IMapper mapeador, IAlmacenadorArchivos almacenador) {
            this.contexto = contexto;
            this.mapeador = mapeador;
            this.almacenador = almacenador;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO) {
            var consultable = contexto.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(consultable);
            var actores = await consultable.OrderBy(g => g.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapeador.Map<List<ActorDTO>>(actores);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO) {
            var actor = mapeador.Map<Actor>(actorCreacionDTO);

            if (actorCreacionDTO.Foto != null) {
                actor.Foto = await almacenador.GuardarArchivo(CONTENEDOR, actorCreacionDTO.Foto);
            }

            await contexto.AddAsync(actor);
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            var existe = await contexto.Actores.AnyAsync(g => g.ID == id);

            if (!existe) { return NotFound(); }

            contexto.Remove(new Actor() { ID = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }

    }

}