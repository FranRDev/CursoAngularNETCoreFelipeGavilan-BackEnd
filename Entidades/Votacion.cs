using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades {

    public class Votacion {

        public int ID { get; set; }
        [Range(1, 5)]
        public int Puntuacion { get; set; }
        public int PeliculaID { get; set; }
        public Pelicula Pelicula { get; set; }
        public string UsuarioID { get; set; }
        public IdentityUser Usuario { get; set; }

    }

}