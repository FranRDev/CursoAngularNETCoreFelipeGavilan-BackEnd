using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades {

    public class Pelicula {

        public int ID { get; set; }
        [Required]
        [StringLength(maximumLength: 300)]
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public string Trailer { get; set; }
        public bool Cartelera { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public string Poster { get; set; }

        public List<PeliculaActor> Actores { get; set; }
        public List<PeliculaCine> Cines { get; set; }
        public List<PeliculaGenero> Generos { get; set; }

    }

}