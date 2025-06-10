using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
// Asegúrate de apuntar al namespace donde esté tu Program:
using task_service_test;

namespace task_service_test
{
    public class TaskServiceIntegrationViaTestServer
    {
        private readonly HttpClient _client;

        public TaskServiceIntegrationViaTestServer()
        {
            // 1. BaseDirectory = …\task-service-test\bin\Debug\net8.0\
            var baseDir = AppContext.BaseDirectory;

            // 2. Sube 4 niveles y entra a "task-service"
            var contentRoot = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "task-service")
            );

            if (!Directory.Exists(contentRoot))
                throw new DirectoryNotFoundException(
                    $"No existe el content root: {contentRoot}"
                );

            // 3. Crea el TestServer apuntando al content root correcto
            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .UseEnvironment("Testing")
                .UseStartup<Program>();   // O UseStartup<Startup> si tu API usa Startup.cs

            var server = new TestServer(builder);
            _client = server.CreateClient();
        }

        [Fact]
        public async Task GetTasks_ReturnsOk()
        {
            var resp = await _client.GetAsync("/api/v1/task");
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreated_AndLocationHeader()
        {
            var nueva = new
            {
                CodigoTarea = "TSK-INT-002",
                Titulo = "Prueba InMem",
                Descripcion = "Creada vía TestServer"
            };
            var contenido = new StringContent(
                JsonSerializer.Serialize(nueva),
                Encoding.UTF8,
                "application/json"
            );

            var resp = await _client.PostAsync("/api/v1/task", contenido);
            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
            Assert.NotNull(resp.Headers.Location);
        }
    }
}
