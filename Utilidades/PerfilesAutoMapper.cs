using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;
using NetTopologySuite.Geometries;

namespace back_end.Utilidades {

    public class PerfilesAutoMapper : Profile {

        public PerfilesAutoMapper(GeometryFactory geometryFactory) {
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(a => a.Foto, opciones => opciones.Ignore());
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Cine, CineDTO>().ForMember(c => c.Latitud, dto => dto.MapFrom(c => c.Ubicacion.Y)).ForMember(c => c.Longitud, dto => dto.MapFrom(c => c.Ubicacion.X));
            CreateMap<CineCreacionDTO, Cine>().ForMember(c => c.Ubicacion, c => c.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));
        }

    }

}