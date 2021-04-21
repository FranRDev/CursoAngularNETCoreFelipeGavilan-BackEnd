using back_end.DTOs;
using back_end.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace back_end.Controllers {

    [Route("api/votaciones")]
    [ApiController]
    public class VotacionesController : ControllerBase {

        private readonly UserManager<IdentityUser> administradorUsuarios;
        private readonly ApplicationDbContext contexto;

        public VotacionesController(UserManager<IdentityUser> administradorUsuarios, ApplicationDbContext contexto) {
            this.administradorUsuarios = administradorUsuarios;
            this.contexto = contexto;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post([FromBody] VotacionDTO votacionDTO) {
            var correo = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var usuario = await administradorUsuarios.FindByEmailAsync(correo);
            var votacionActual = await contexto.Votaciones.FirstOrDefaultAsync(v => v.PeliculaID == votacionDTO.PeliculaID && v.UsuarioID == usuario.Id);

            if (votacionActual == null) {
                await contexto.AddAsync(new Votacion() {
                    PeliculaID = votacionDTO.PeliculaID,
                    Puntuacion = votacionDTO.Puntuacion,
                    UsuarioID = usuario.Id
                });

            } else {
                votacionActual.Puntuacion = votacionDTO.Puntuacion;
            }

            await contexto.SaveChangesAsync();
            return Ok();
        }

    }

}