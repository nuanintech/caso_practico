using FluentFTP;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.Json;
using task_service.Application.Commands;
using task_service.Domain.Exceptions;
using task_service.Infrastructure.Configuration;
using task_service.Infrastructure.Ftp;
using task_service.Shared.DTOs;

namespace task_service.Infrastructure.Services {
    public class FtpTareaProcesarServicio : BackgroundService{
        #region Atributos 
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<FtpTareaProcesarServicio> logger;
        private readonly FtpSettings ftpSettings;
        private readonly FtpHelper ftpHelper;
        #endregion

        #region Constructores
        public FtpTareaProcesarServicio(IServiceProvider serviceProvider, ILogger<FtpTareaProcesarServicio> logger, IOptions<FtpSettings> ftpOptions, FtpHelper ftpHelper) {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.ftpSettings = ftpOptions.Value;
            this.ftpHelper = ftpHelper;
        }
        #endregion

        #region Métodos
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                try {
                    // ✅ Aquí llamas al helper y le pasas la función que procesa cada archivo
                    await ftpHelper.ProcessFileAsync(ProcesarJsonFileAsync, logger, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error procesando archivos desde FTP.");
                }

                await Task.Delay(TimeSpan.FromMinutes(ftpSettings.IntervaloMinutos), stoppingToken);
            }
        }
        private async Task ProcesarJsonFileAsync(string nombreArchivo, string contenidoJson, AsyncFtpClient client) {
            var exitosas = new List<CreateTareaDTO>();
            var fallidas = new List<object>();

            try {
                var tareas = JsonSerializer.Deserialize<List<CreateTareaDTO>>(contenidoJson);

                if (tareas != null) {
                    using var scope = serviceProvider.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                    foreach (var tareaDTO in tareas) {
                        try {
                           
                            var command = new CreateTareaCommand(tareaDTO);
                            var tarea = await mediator.Send(command);
                            exitosas.Add(tareaDTO);
                        }
                        catch (Domain.Exceptions.ValidationException ex) {
                            var errores = ex.Errors?.SelectMany(e => e.Value).ToArray() ?? new[] { ex.Message };
                            AddError(fallidas, tareaDTO, "Se encontraron errores de validación", errores);
                        }
                        catch (NotFoundException ex) {
                            AddError(fallidas, tareaDTO, "Recurso no encontrado", new[] { ex.Message });
                        }
                        catch (ConflictException ex) {
                            AddError(fallidas, tareaDTO, "Dato inválido", new[] { ex.Message });
                        }
                        catch (Exception ex) {
                            // Captura errores anidados si existen
                            var detalles = new List<string> { ex.Message };
                            if (ex.InnerException != null)
                                detalles.Add(ex.InnerException.Message);

                            AddError(fallidas, tareaDTO, "Error inesperado", detalles.ToArray());
                        }
                    }

                    // Guardar resultados
                    if (exitosas.Any())
                        await ftpHelper.UploadResultAsync(client, nombreArchivo, exitosas, esError: false);

                    if (fallidas.Any())
                        await ftpHelper.UploadResultAsync(client, nombreArchivo, fallidas, esError: true);

                    await ftpHelper.DeleteFileAsync(client, nombreArchivo); // limpia original
                }
            }
            catch (Exception ex) {
                logger.LogWarning(ex, "⚠️ JSON inválido en archivo: {Archivo}", nombreArchivo);
                throw;
            }
        }
        private void AddError(List<object> fallidas, CreateTareaDTO tareaDTO, string mensaje, string[] detalles) {
            fallidas.Add(new {
                tarea = tareaDTO,
                error = new {
                    mensaje,
                    detalles
                }
            });
        }
        #endregion
    }
}
