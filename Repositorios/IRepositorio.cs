using back_end.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Repositorios {

    public interface IRepositorio {

        List<Genero> ObtenerGeneros();
        Task<Genero> ObtenerGeneroPorId(int ID);

    }

}