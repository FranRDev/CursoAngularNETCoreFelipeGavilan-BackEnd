using back_end.Entidades;
using System.Collections.Generic;

namespace back_end.Repositorios {

    public class RepositorioEnMemoria {

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

    }

}