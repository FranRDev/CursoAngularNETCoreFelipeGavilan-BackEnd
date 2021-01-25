using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace back_end.Utilidades {

    public class AlmacenadorAzureStorage : IAlmacenadorArchivos {

        private readonly string cadenaDeConexion;

        public AlmacenadorAzureStorage(IConfiguration configuracion) {
            cadenaDeConexion = configuracion.GetConnectionString("CadenaConexionAzureStorage");
        }

        public async Task BorrarArchivo(string ruta, string contenedor) {
            if (string.IsNullOrEmpty(ruta)) { return; }

            var cliente = new BlobContainerClient(cadenaDeConexion, contenedor);
            await cliente.CreateIfNotExistsAsync();

            var blob = cliente.GetBlobClient(Path.GetFileName(ruta));
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditarArchivo(string ruta, string contenedor, IFormFile archivo) {
            await BorrarArchivo(ruta, contenedor);
            return await GuardarArchivo(contenedor, archivo);
        }

        public async Task<string> GuardarArchivo(string contenedor, IFormFile archivo) {
            var cliente = new BlobContainerClient(cadenaDeConexion, contenedor);
            await cliente.CreateIfNotExistsAsync();
            await cliente.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var blob = cliente.GetBlobClient($"{Guid.NewGuid()}{Path.GetExtension(archivo.FileName)}");
            await blob.UploadAsync(archivo.OpenReadStream());
            return blob.Uri.ToString();
        }

    }

}