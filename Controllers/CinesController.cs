using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace back_end.Controllers {

    [Route("api/cines")]
    [ApiController]
    public class CinesController : ControllerBase {

        private readonly IMapper mapeador;
        private readonly ApplicationDbContext contexto;

        public CinesController(ApplicationDbContext contexto, IMapper mapeador) {
            this.contexto = contexto;
            this.mapeador = mapeador;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineCreacionDTO cineCreacionDTO) {
            contexto.Add(mapeador.Map<Cine>(cineCreacionDTO));
            await contexto.SaveChangesAsync();
            return NoContent();
        }

    }

}