using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Utilidades {

    public static class ExtensionHttpContext {

        public async static Task InsertarParametrosPaginacionEnCabecera<T>(this HttpContext contextoHttp, IQueryable<T> consultable) {
            if (contextoHttp == null) { throw new ArgumentNullException(nameof(contextoHttp)); }

            double cantidad = await consultable.CountAsync();

            contextoHttp.Response.Headers.Add("Total-Registros", cantidad.ToString());
        }

    }

}