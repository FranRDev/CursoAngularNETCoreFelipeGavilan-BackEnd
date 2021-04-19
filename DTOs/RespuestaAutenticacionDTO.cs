using System;

namespace back_end.DTOs {

    public class RespuestaAutenticacionDTO {

        public string Token { get; set; }
        public DateTime Expiracion { get; set; }

    }

}