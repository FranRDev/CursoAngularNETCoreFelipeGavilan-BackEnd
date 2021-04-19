using System.ComponentModel.DataAnnotations;

namespace back_end.DTOs {

    public class CredencialesUsuarioDTO {

        [EmailAddress]
        [Required]
        public string Correo { get; set; }
        [Required]
        public string Clave { get; set; }

    }

}