using back_end.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace back_end.Repositorios {

    public class RepositorioEnMemoria: IRepositorio {

        private List<Genero> generos;

        public RepositorioEnMemoria() {
            generos = new List<Genero>() {
                new Genero() { ID = 1, Nombre = "Comedia"},
                new Genero() { ID = 2, Nombre = "Acción"}
            };
        }

        public List<Genero> ObtenerGeneros() {
            return generos;
        }

        public Genero ObtenerGeneroPorId(int ID) {
            return generos.FirstOrDefault(generos => generos.ID == ID);
        }

    }

}