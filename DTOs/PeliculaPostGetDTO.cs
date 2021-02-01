using System.Collections.Generic;

namespace back_end.DTOs {

    public class PeliculaPostGetDTO {

        public List<CineDTO> Cines { get; set; }
        public List<GeneroDTO> Generos { get; set; }

    }

}