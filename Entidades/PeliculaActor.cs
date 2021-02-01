using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades {

    public class PeliculaActor {

        public int IdPelicula { get; set; }
        public int IdActor { get; set; }
        [StringLength(maximumLength: 100)]
        public string Personaje { get; set; }
        public int Orden { get; set; }
        public Pelicula Pelicula { get; set; }
        public Actor Actor { get; set; }

    }

}