using Microsoft.Extensions.Options;
using System.Net.Http;
using task_service.Domain.Interfaces;
using task_service.Infrastructure.Configuration;

namespace task_service.Infrastructure.Services
{
    public class UsuarioServicio : IUsuarioService {
        private readonly HttpClient httpClient;
        private readonly ClientSettings clientSettings;
        private readonly ILogger<UsuarioServicio> logger;

        public UsuarioServicio(HttpClient httpClient, IOptions<ClientSettings> options, ILogger<UsuarioServicio> logger) {
            this.httpClient = httpClient;
            this.clientSettings = options.Value;
            this.logger = logger;
        }

        public async Task<bool>ValidateClientExistenceAsync(Guid usuarioId) {
            try {
                var url = clientSettings.ClientByIdUrl.Replace("{usuarioId}", usuarioId.ToString());

                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                    return true;

                logger.LogWarning("Validación fallida: el usuario {UsuarioId} no existe. Status: {StatusCode}", usuarioId, response.StatusCode);
                return false;
            }
            catch (Exception ex) {
                logger.LogError(ex, "Error al validar existencia del usuario {UsuarioId}", usuarioId);
                return false;
            }
        }
    }
}
