using System;
using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs {

    public class VotacionDTO {

        [Range(1, 5)]
        public int Puntuacion { get; set; }
        public int PeliculaID { get; set; }

    }

}