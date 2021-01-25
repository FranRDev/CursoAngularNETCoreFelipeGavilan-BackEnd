using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace back_end.Utilidades {

    public class AlmacenadorArchivosLocal : IAlmacenadorArchivos {

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment entorno;

        public AlmacenadorArchivosLocal(IWebHostEnvironment entorno, IHttpContextAccessor httpContextAccessor) {
            this.entorno = entorno;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task BorrarArchivo(string ruta, string contenedor) {
            if (string.IsNullOrEmpty(ruta)) { return Task.CompletedTask; }

            string nombreArchivo = Path.GetFileName(ruta);
            string directorioArchivo = Path.Combine(entorno.WebRootPath, contenedor, nombreArchivo);

            if (File.Exists(directorioArchivo)) { File.Delete(directorioArchivo); }

            return Task.CompletedTask;
        }

        public async Task<string> EditarArchivo(string ruta, string contenedor, IFormFile archivo) {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenedor, archivo);
        }

        public async Task<string> GuardarArchivo(string contenedor, IFormFile archivo) {
            string carpeta = Path.Combine(entorno.WebRootPath, contenedor);
            if (!Directory.Exists(carpeta)) { Directory.CreateDirectory(carpeta); }

            string nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}";
            string ruta = Path.Combine(carpeta, nombreArchivo);

            using (var ms = new MemoryStream()) {
                await archivo.CopyToAsync(ms);
                var contenido = ms.ToArray();
                await File.WriteAllBytesAsync(ruta, contenido);
            }

            string urlActual = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            string rutaBDD = Path.Combine(urlActual, contenedor, nombreArchivo).Replace("\\", "/");

            return rutaBDD;
        }

    }

}