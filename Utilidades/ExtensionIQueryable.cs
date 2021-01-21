using back_end.DTOs;
using System.Linq;

namespace back_end.Utilidades {

    public static class ExtensionIQueryable {

        public static IQueryable<T> Paginar<T>(this IQueryable<T> consultable, PaginacionDTO paginacionDTO) {
            return consultable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.Registros)
                .Take(paginacionDTO.Registros);
        }

    }

}