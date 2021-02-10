using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using back_end.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public async Task<ActionResult<PaginaInicioDTO>> Get() {
            var registros = 6;
            var hoy = DateTime.Today;

            var enCartelera = await contexto.Peliculas
                .Where(p => p.Cartelera)
                .OrderBy(p => p.FechaLanzamiento)
                .Take(registros)
                .ToListAsync();

            var proximosEstrenos = await contexto.Peliculas
                .Where(p => p.FechaLanzamiento > hoy)
                .OrderBy(p => p.FechaLanzamiento)
                .Take(registros)
                .ToListAsync();

            return new PaginaInicioDTO() {
                EnCartelera = mapeador.Map<List<PeliculaDTO>>(enCartelera),
                ProximosEstrenos = mapeador.Map<List<PeliculaDTO>>(proximosEstrenos)
            };
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id) {
            var pelicula = await contexto.Peliculas
                .Include(p => p.Actores).ThenInclude(a => a.Actor)
                .Include(p => p.Cines).ThenInclude(c => c.Cine)
                .Include(p => p.Generos).ThenInclude(g => g.Genero)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (pelicula == null) { return NotFound(); }

            var dto = mapeador.Map<PeliculaDTO>(pelicula);
            dto.Actores = dto.Actores.OrderBy(a => a.Orden).ToList();
            return dto;
        }

        [HttpGet("PostGet")]
        public async Task<ActionResult<PeliculaPostGetDTO>> PostGet() {
            return new PeliculaPostGetDTO() {
                Cines = mapeador.Map<List<CineDTO>>(await contexto.Cines.ToListAsync()),
                Generos = mapeador.Map<List<GeneroDTO>>(await contexto.Generos.ToListAsync())
            };
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