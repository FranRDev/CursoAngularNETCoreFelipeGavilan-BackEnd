using back_end.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Repositorios {

    public class RepositorioEnMemoria: IRepositorio {

        private List<Genero> generos;

        public Guid guid;

        public RepositorioEnMemoria() {
            generos = new List<Genero>() {
                new Genero() { ID = 1, Nombre = "Comedia"},
                new Genero() { ID = 2, Nombre = "Acción"}
            };

            guid = Guid.NewGuid();
        }

        public List<Genero> ObtenerGeneros() {
            return generos;
        }

        public async Task<Genero> ObtenerGeneroPorId(int ID) {
            await Task.Delay(1);
            return generos.FirstOrDefault(generos => generos.ID == ID);
        }

        public Guid ObtenerGuid() {
            return guid;
        }

        public void CrearGenero(Genero genero) {
            genero.ID = generos.Count + 1;
            generos.Add(genero);
        }

    }

}