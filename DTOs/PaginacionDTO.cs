namespace back_end.DTOs {

    public class PaginacionDTO {

        private const int MAXIMO_REGISTROS = 50;

        private int registros = 10;

        public int Pagina { get; set; } = 1;

        public int Registros {
            get { return registros; }
            set { registros = (value > MAXIMO_REGISTROS) ? MAXIMO_REGISTROS : value; }
        }

    }

}