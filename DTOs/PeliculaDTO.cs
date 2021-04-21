using System;
using System.Collections.Generic;

namespace back_end.DTOs {

    public class PeliculaDTO {

        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public string Trailer { get; set; }
        public bool Cartelera { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string Poster { get; set; }
        public double MediaVotacion { get; set; }
        public int PuntuacionUsuario { get; set; }

        public List<PeliculaActorDTO> Actores { get; set; }
        public List<CineDTO> Cines { get; set; }
        public List<GeneroDTO> Generos { get; set; }

    }

}