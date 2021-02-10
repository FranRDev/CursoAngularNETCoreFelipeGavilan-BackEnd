using System.Collections.Generic;

namespace back_end.DTOs {

    public class PaginaInicioDTO {

        public List<PeliculaDTO> EnCartelera { get; set; }
        public List<PeliculaDTO> ProximosEstrenos { get; set; }

    }

}