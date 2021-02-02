using back_end.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs {

    public class PeliculaCreacionDTO {

        [Required]
        [StringLength(maximumLength: 300)]
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public string Trailer { get; set; }
        public bool Cartelera { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public IFormFile Poster { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculaCreacionDTO>>))]
        public List<ActorPeliculaCreacionDTO> Actores { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> IdsCines { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> IdsGeneros { get; set; }

    }

}