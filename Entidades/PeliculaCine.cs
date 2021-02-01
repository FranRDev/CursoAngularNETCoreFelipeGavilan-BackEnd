namespace back_end.Entidades {

    public class PeliculaCine {

        public int IdPelicula { get; set; }
        public int IdCine { get; set; }
        public Pelicula Pelicula { get; set; }
        public Cine Cine { get; set; }

    }

}