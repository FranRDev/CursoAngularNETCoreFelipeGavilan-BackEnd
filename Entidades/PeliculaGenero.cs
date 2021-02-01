namespace back_end.Entidades {

    public class PeliculaGenero {

        public int IdPelicula { get; set; }
        public int IdGenero { get; set; }
        public Pelicula Pelicula { get; set; }
        public Genero Genero { get; set; }

    }

}