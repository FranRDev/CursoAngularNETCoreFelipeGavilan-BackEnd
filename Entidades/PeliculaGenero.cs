namespace back_end.Entidades {

    public class PeliculaGenero {

        public int PeliculaID { get; set; }
        public int GeneroID { get; set; }
        public Pelicula Pelicula { get; set; }
        public Genero Genero { get; set; }

    }

}