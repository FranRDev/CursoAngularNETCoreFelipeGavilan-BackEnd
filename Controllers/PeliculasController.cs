using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace back_end.Controllers {

    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase {

        private const string CONTENEDOR = "peliculas";

        private readonly IAlmacenadorArchivos almacenador;
        private readonly IMapper mapeador;
        private readonly ApplicationDbContext contexto;

        public PeliculasController(ApplicationDbContext contexto, IMapper mapeador, IAlmacenadorArchivos almacenador) {
            this.contexto = contexto;
            this.mapeador = mapeador;
            this.almacenador = almacenador;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO) {
            var pelicula = mapeador.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null) {
                pelicula.Poster = await almacenador.GuardarArchivo(CONTENEDOR, peliculaCreacionDTO.Poster);
            }

            EscribirOrdenActores(pelicula);

            contexto.Add(pelicula);
            await contexto.SaveChangesAsync();
            return NoContent();
        }

        private static void EscribirOrdenActores(Pelicula pelicula) {
            if (pelicula.Actores != null) {
                for (int contador = 0; contador < pelicula.Actores.Count; contador++) {
                    pelicula.Actores[contador].Orden = contador;
                }
            }
        }

    }

}