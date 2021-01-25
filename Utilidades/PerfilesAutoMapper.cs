using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;

namespace back_end.Utilidades {

    public class PerfilesAutoMapper : Profile {

        public PerfilesAutoMapper() {
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(a => a.Foto, opciones => opciones.Ignore());
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
        }

    }

}