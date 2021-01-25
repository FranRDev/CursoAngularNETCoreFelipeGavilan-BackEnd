using AutoMapper;
using back_end.DTOs;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
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

    }

}