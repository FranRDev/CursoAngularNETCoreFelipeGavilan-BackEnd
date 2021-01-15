using back_end.Repositorios;

namespace back_end.Controllers {

    public class ControladorGeneros {

        private readonly IRepositorio repositorio;

        public ControladorGeneros(IRepositorio repositorio) {
            this.repositorio = repositorio;
        }

    }

}