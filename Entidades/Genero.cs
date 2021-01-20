using back_end.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace back_end.Entidades {

    public class Genero {

        public int ID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(maximumLength: 50)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

    }

}