using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace back_end.Utilidades {

    public class PerfilesAutoMapper : Profile {

        public PerfilesAutoMapper(GeometryFactory geometryFactory) {
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(a => a.Foto, opciones => opciones.Ignore());
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Cine, CineDTO>().ForMember(c => c.Latitud, dto => dto.MapFrom(c => c.Ubicacion.Y)).ForMember(c => c.Longitud, dto => dto.MapFrom(c => c.Ubicacion.X));
            CreateMap<CineCreacionDTO, Cine>().ForMember(c => c.Ubicacion, c => c.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(p => p.Poster, opciones => opciones.Ignore())
                .ForMember(p => p.Actores, opciones => opciones.MapFrom(MapearPeliculasActores))
                .ForMember(p => p.Cines, opciones => opciones.MapFrom(MapearPeliculasCines))
                .ForMember(p => p.Generos, opciones => opciones.MapFrom(MapearPeliculasGeneros));
        }

        private List<PeliculaActor> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula) {
            var resultado = new List<PeliculaActor>();

            if (peliculaCreacionDTO.IdsActores == null) { return resultado; }

            foreach (ActorPeliculaCreacionDTO actor in peliculaCreacionDTO.IdsActores) { resultado.Add(new PeliculaActor() { IdActor = actor.ID, Personaje = actor.Personaje }); }

            return resultado;
        }

        private List<PeliculaCine> MapearPeliculasCines(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula) {
            var resultado = new List<PeliculaCine>();

            if (peliculaCreacionDTO.IdsCines == null) { return resultado; }

            foreach (int id in peliculaCreacionDTO.IdsCines) { resultado.Add(new PeliculaCine() { IdCine = id }); }

            return resultado;
        }

        private List<PeliculaGenero> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula) {
            var resultado = new List<PeliculaGenero>();

            if (peliculaCreacionDTO.IdsGeneros == null) { return resultado; }

            foreach (int id in peliculaCreacionDTO.IdsGeneros) { resultado.Add(new PeliculaGenero() { IdGenero = id }); }

            return resultado;
        }

    }

}