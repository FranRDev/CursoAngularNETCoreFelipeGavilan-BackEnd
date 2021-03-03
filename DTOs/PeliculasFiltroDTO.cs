namespace back_end.DTOs {

    public class PeliculasFiltroDTO {

        public int Pagina { get; set; }
        public int Registros { get; set; }
        public PaginacionDTO Paginacion { get => new PaginacionDTO() { Pagina = Pagina, Registros = Registros }; }
        public string Titulo { get; set; }
        public int IdGenero { get; set; }
        public bool EnCartelera { get; set; }
        public bool ProximosEstrenos { get; set; }

    }

}