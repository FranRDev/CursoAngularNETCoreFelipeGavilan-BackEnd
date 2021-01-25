using AutoMapper;
using back_end.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace back_end.Controllers {

    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase {

        private readonly IMapper mapeador;
        private readonly ApplicationDbContext contexto;

        public ActoresController(ApplicationDbContext contexto, IMapper mapeador) {
            this.contexto = contexto;
            this.mapeador = mapeador;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ActorCreacionDTO actorCreacionDTO) {
            var actor = mapeador.Map<Actor>(actorCreacionDTO);
            await contexto.AddAsync(actor);
            await contexto.SaveChangesAsync();
            return NoContent();
        }

    }

}