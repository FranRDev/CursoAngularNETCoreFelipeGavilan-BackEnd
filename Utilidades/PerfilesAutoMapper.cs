using AutoMapper;
using back_end.DTOs;
using back_end.Entidades;

namespace back_end.Utilidades {

    public class PerfilesAutoMapper : Profile {

        public PerfilesAutoMapper() {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
        }

    }

}