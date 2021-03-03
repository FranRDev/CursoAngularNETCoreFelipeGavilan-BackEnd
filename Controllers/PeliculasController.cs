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

        [HttpGet("Filtrar")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] PeliculasFiltroDTO peliculasFiltroDTO) {
            var peliculasConsultable = contexto.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(peliculasFiltroDTO.Titulo)) {
                peliculasConsultable = peliculasConsultable.Where(p => p.Titulo.Contains(peliculasFiltroDTO.Titulo));
            }

            if (peliculasFiltroDTO.EnCartelera) {
                peliculasConsultable = peliculasConsultable.Where(p => p.Cartelera);
            }

            if (peliculasFiltroDTO.ProximosEstrenos) {
                var hoy = DateTime.Today;
                peliculasConsultable = peliculasConsultable.Where(p => p.FechaLanzamiento > hoy);
            }

            if (peliculasFiltroDTO.IdGenero != 0) {
                peliculasConsultable = peliculasConsultable.Where(p => p.Generos.Select(g => g.GeneroID).Contains(peliculasFiltroDTO.IdGenero));
            }

            await HttpContext.InsertarParametrosPaginacionEnCabecera(peliculasConsultable);

            var peliculas = await peliculasConsultable.Paginar(peliculasFiltroDTO.Paginacion).ToListAsync();
            return mapeador.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO) {
            var pelicula = mapeador.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null) {
                pelicula.Poster = await almacenador.GuardarArchivo(CONTENEDOR, peliculaCreacionDTO.Poster);
            }

            EscribirOrdenActores(pelicula);

            contexto.Add(pelicula);
            await contexto.SaveChangesAsync();
            return pelicula.ID;
        }

        private static void EscribirOrdenActores(Pelicula pelicula) {
            if (pelicula.Actores != null) {
                for (int contador = 0; contador < pelicula.Actores.Count; contador++) {
                    pelicula.Actores[contador].Orden = contador;
                }
            }
        }

        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<PeliculaPutGetDTO>> PutGet(int id) {
            var peliculaAR = await Get(id);
            if (peliculaAR.Result is NotFoundResult) { return NotFound(); }

            var pelicula = peliculaAR.Value;

            var idsGenerosSeleccionados = pelicula.Generos.Select(g => g.ID).ToList();
            var generosNoSeleccionados = await contexto.Generos.Where(g => !idsGenerosSeleccionados.Contains(g.ID)).ToListAsync();

            var idsCinesSeleccionados = pelicula.Cines.Select(c => c.ID).ToList();
            var cinesNoSeleccionados = await contexto.Cines.Where(c => !idsCinesSeleccionados.Contains(c.ID)).ToListAsync();

            return new PeliculaPutGetDTO() {
                Pelicula = pelicula,
                GenerosSeleccionados = pelicula.Generos,
                GenerosNoSeleccionados = mapeador.Map<List<GeneroDTO>>(generosNoSeleccionados),
                CinesSeleccionados = pelicula.Cines,
                CinesNoSeleccionados = mapeador.Map<List<CineDTO>>(cinesNoSeleccionados),
                Actores = pelicula.Actores
            };
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO) {
            var pelicula = await contexto.Peliculas
                .Include(p => p.Actores)
                .Include(p => p.Cines)
                .Include(p => p.Generos)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (pelicula == null) { return NotFound(); }

            pelicula = mapeador.Map(peliculaCreacionDTO, pelicula);

            if (peliculaCreacionDTO.Poster != null) {
                pelicula.Poster = await almacenador.EditarArchivo(pelicula.Poster, CONTENEDOR, peliculaCreacionDTO.Poster);
            }

            EscribirOrdenActores(pelicula);

            await contexto.SaveChangesAsync();
            return NoContent();
        }

    }

}