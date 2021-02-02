using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int id) {
            var actor = await contexto.Actores.FirstOrDefaultAsync(g => g.ID == id);

            if (actor == null) { return NotFound(); }

            return mapeador.Map<ActorDTO>(actor);
        }

        [HttpPost("ObtenerPorNombre")]
        public async Task<ActionResult<List<PeliculaActorDTO>>> ObtenerPorNombre([FromBody] string nombre) {
            if (string.IsNullOrEmpty(nombre)) { return new List<PeliculaActorDTO>(); }

            return await contexto.Actores
                .Where(a => a.Nombre.Contains(nombre))
                .Select(a => new PeliculaActorDTO { ID = a.ID, Nombre = a.Nombre, Foto = a.Foto })
                .Take(5)
                .ToListAsync();
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

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] ActorCreacionDTO actorCreacionDTO) {
            var actor = await contexto.Actores.FirstOrDefaultAsync(g => g.ID == id);

            if (actor == null) { return NotFound(); }

            actor = mapeador.Map(actorCreacionDTO, actor);

            if (actorCreacionDTO.Foto != null) {
                actor.Foto = await almacenador.EditarArchivo(actor.Foto, CONTENEDOR, actorCreacionDTO.Foto);
            }

            await contexto.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            var actor = await contexto.Actores.FirstOrDefaultAsync(g => g.ID == id);

            if (actor == null) { return NotFound(); }

            contexto.Remove(actor);
            await contexto.SaveChangesAsync();

            await almacenador.BorrarArchivo(actor.Foto, CONTENEDOR);

            return NoContent();
        }

    }

}