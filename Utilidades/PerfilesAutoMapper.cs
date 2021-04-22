using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace back_end.Utilidades {

    public class PerfilesAutoMapper : Profile {

        public PerfilesAutoMapper(GeometryFactory geometryFactory) {
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(a => a.Foto, opciones => opciones.Ignore());
            CreateMap<Cine, CineDTO>().ForMember(c => c.Latitud, dto => dto.MapFrom(c => c.Ubicacion.Y)).ForMember(c => c.Longitud, dto => dto.MapFrom(c => c.Ubicacion.X));
            CreateMap<CineCreacionDTO, Cine>().ForMember(c => c.Ubicacion, c => c.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<IdentityUser, UsuarioDTO>()
                .ForMember(u => u.Correo, opciones => opciones.MapFrom(iu => iu.Email));
            CreateMap<Pelicula, PeliculaDTO>()
                .ForMember(p => p.Actores, opciones => opciones.MapFrom(MapearPeliculasActores))
                .ForMember(p => p.Cines, opciones => opciones.MapFrom(MapearPeliculasCines))
                .ForMember(p => p.Generos, opciones => opciones.MapFrom(MapearPeliculasGeneros));
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(p => p.Poster, opciones => opciones.Ignore())
                .ForMember(p => p.Actores, opciones => opciones.MapFrom(MapearPeliculasActores))
                .ForMember(p => p.Cines, opciones => opciones.MapFrom(MapearPeliculasCines))
                .ForMember(p => p.Generos, opciones => opciones.MapFrom(MapearPeliculasGeneros));
        }

        private List<PeliculaActorDTO> MapearPeliculasActores(Pelicula pelicula, PeliculaDTO peliculaDTO) {
            var resultado = new List<PeliculaActorDTO>();

            if (pelicula.Actores != null) {
                foreach (var actor in pelicula.Actores) {
                    resultado.Add(new PeliculaActorDTO() { ID = actor.ActorID, Nombre = actor.Actor.Nombre, Foto = actor.Actor.Foto, Orden = actor.Orden, Personaje = actor.Personaje });
                }
            }

            return resultado;
        }

        private List<PeliculaActor> MapearPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula) {
            var resultado = new List<PeliculaActor>();

            if (peliculaCreacionDTO.Actores == null) { return resultado; }

            foreach (ActorPeliculaCreacionDTO actor in peliculaCreacionDTO.Actores) { resultado.Add(new PeliculaActor() { ActorID = actor.ID, Personaje = actor.Personaje }); }

            return resultado;
        }

        private List<CineDTO> MapearPeliculasCines(Pelicula pelicula, PeliculaDTO peliculaDTO) {
            var resultado = new List<CineDTO>();

            if (pelicula.Cines != null) {
                foreach (var cine in pelicula.Cines) {
                    resultado.Add(new CineDTO() { ID = cine.CineID, Nombre = cine.Cine.Nombre, Latitud = cine.Cine.Ubicacion.Y, Longitud = cine.Cine.Ubicacion.X });
                }
            }

            return resultado;
        }

        private List<PeliculaCine> MapearPeliculasCines(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula) {
            var resultado = new List<PeliculaCine>();

            if (peliculaCreacionDTO.IdsCines == null) { return resultado; }

            foreach (int id in peliculaCreacionDTO.IdsCines) { resultado.Add(new PeliculaCine() { CineID = id }); }

            return resultado;
        }

        private List<GeneroDTO> MapearPeliculasGeneros(Pelicula pelicula, PeliculaDTO peliculaDTO) {
            var resultado = new List<GeneroDTO>();

            if (pelicula.Generos != null) {
                foreach (var genero in pelicula.Generos) {
                    resultado.Add(new GeneroDTO() { ID = genero.GeneroID, Nombre = genero.Genero.Nombre });
                }
            }

            return resultado;
        }

        private List<PeliculaGenero> MapearPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula) {
            var resultado = new List<PeliculaGenero>();

            if (peliculaCreacionDTO.IdsGeneros == null) { return resultado; }

            foreach (int id in peliculaCreacionDTO.IdsGeneros) { resultado.Add(new PeliculaGenero() { GeneroID = id }); }

            return resultado;
        }

    }

}