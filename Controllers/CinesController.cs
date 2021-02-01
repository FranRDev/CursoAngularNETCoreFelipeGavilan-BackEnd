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

    [Route("api/cines")]
    [ApiController]
    public class CinesController : ControllerBase {

        private readonly IMapper mapeador;
        private readonly ApplicationDbContext contexto;

        public CinesController(ApplicationDbContext contexto, IMapper mapeador) {
            this.contexto = contexto;
            this.mapeador = mapeador;
        }

        [HttpGet]
        public async Task<ActionResult<List<CineDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO) {
            var consultable = contexto.Cines.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(consultable);
            var cines = await consultable.OrderBy(c => c.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapeador.Map<List<CineDTO>>(cines);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CineCreacionDTO cineCreacionDTO) {
            contexto.Add(mapeador.Map<Cine>(cineCreacionDTO));
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] CineCreacionDTO cineCreacionDTO) {
            var cine = await contexto.Cines.FirstOrDefaultAsync(g => g.ID == id);

            if (cine == null) { return NotFound(); }

            cine = mapeador.Map(cineCreacionDTO, cine);

            await contexto.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            var existe = await contexto.Cines.AnyAsync(g => g.ID == id);

            if (!existe) { return NotFound(); }

            contexto.Remove(new Cine() { ID = id });
            await contexto.SaveChangesAsync();
            return NoContent();
        }

    }

}