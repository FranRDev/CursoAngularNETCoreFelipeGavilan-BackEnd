using System;
using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs {

    public class ActorCreacionDTO {

        public int ID { get; set; }
        [Required]
        [StringLength(maximumLength: 200)]
        public string Nombre { get; set; }
        public string Biografia { get; set; }
        public DateTime FechaNacimiento { get; set; }

    }

}