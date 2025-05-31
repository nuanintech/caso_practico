using FluentFTP;
using System.Net;
using System.Text.Json;
using task_service.Infrastructure.Configuration;
using task_service.Infrastructure.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace task_service.Infrastructure.Ftp
{
    public class FtpHelper {
        #region Atributos
        private readonly FtpSettings ftpSettings;
        #endregion

        #region Constructores
        public FtpHelper(FtpSettings settings) {
            this.ftpSettings = settings;
        }
        #endregion

        #region Métodos
        private AsyncFtpClient CreateConnection() {
            var credentials = new NetworkCredential(ftpSettings.Usuario, ftpSettings.Contrasena);
            var client = new AsyncFtpClient(ftpSettings.Host, credentials);
            // Puedes ajustar el timeout aquí si es necesario
            client.Config.ReadTimeout = 60000;
            client.Config.DataConnectionConnectTimeout = 60000;
            client.Config.SocketKeepAlive = true;

            return client;
        }
        public async Task ProcessFileAsync(Func<string, string, AsyncFtpClient, Task> processJsonFileAsync, ILogger logger, CancellationToken cancellationToken = default) {
            await using var client = CreateConnection();

            try {
                await client.Connect();

                if (client.IsConnected)
                    Console.WriteLine("Conectado correctamente al FTP.");
            }
            catch (Exception ex) {
               logger.LogError($"more2286: Error al conectar al FTP: {ex.Message}");
            }

            try {
                var archivos = await GetFilesJsonAsync(client, cancellationToken);

                foreach (var archivo in archivos) {
                    try {
                        var contenido = await DownloadFileByTextAsync(client, archivo, cancellationToken);

                        // Aquí se llama tu función externa (proceso del JSON)
                        await processJsonFileAsync(archivo, contenido, client);

                        //await MoverArchivoProcesadoAsync(client, archivo);
                    }
                    catch (Exception exArchivo) {
                        logger.LogWarning(exArchivo, "more2286: Archivo con estructura incorrecta: {Archivo}", archivo);
                        await MoveFileFailedAsync(client, archivo);
                    }
                }
            }
            catch (Exception exConexion) {
                logger.LogError(exConexion, "more2286: Error conectando o listando archivos en el FTP");
            }
        }
        public async Task<IEnumerable<string>> GetFilesJsonAsync(AsyncFtpClient client, CancellationToken cancellationToken) {
            var items = await client.GetListing(ftpSettings.RutaPendientes, cancellationToken);
            return items
                .Where(i => i.Type == FtpObjectType.File && i.FullName.EndsWith(".json"))
                .Select(i => Path.GetFileName(i.FullName));
        }
        private async Task<string> DownloadFileByTextAsync(AsyncFtpClient client, string nombreArchivo, CancellationToken token) {
            var rutaCompleta = $"{ftpSettings.RutaPendientes}{nombreArchivo}";

            // Usamos DownloadBytes para tener control total
            var data = await client.DownloadBytes(rutaCompleta, token: token);

            return System.Text.Encoding.UTF8.GetString(data);
        }
        public async Task UploadResultAsync(AsyncFtpClient client, string fileName, object contenido, bool esError) {
            var nuevoNombre = $"{Path.GetFileNameWithoutExtension(fileName)}_{(esError ? "errores" : "ok")}.json";
            var destino = esError ? ftpSettings.RutaErrores : ftpSettings.RutaProcesados;
            var pathRemoto = $"{destino}{nuevoNombre}";

            var json = JsonSerializer.Serialize(contenido, new JsonSerializerOptions { WriteIndented = true });
            var tempFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try {
                await File.WriteAllTextAsync(tempFile, json);
                await client.UploadFile(tempFile, pathRemoto);
            }
            finally {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }
        public async Task DeleteFileAsync(AsyncFtpClient client, string fileName) {
            await client.DeleteFile($"{ftpSettings.RutaPendientes}{fileName}");
        }
        public async Task MoveFileFailedAsync(AsyncFtpClient client, string fileName) {
            var nuevoNombre = $"{Path.GetFileNameWithoutExtension(fileName)}_malformato.json";
            var origen = $"{ftpSettings.RutaPendientes}{fileName}";
            var destino = $"{ftpSettings.RutaErrores}{nuevoNombre}";

            // Asegurar que el directorio destino exista
            if (!await client.DirectoryExists(ftpSettings.RutaErrores)) {
                await client.CreateDirectory(ftpSettings.RutaErrores, true);
            }

            await client.MoveFile(origen, destino, FtpRemoteExists.Overwrite);
        }
        #endregion
    }
}
