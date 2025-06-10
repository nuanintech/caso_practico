using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;                                 // Xunit para [Fact]
using task_service.Shared.DTOs;

namespace task_service_test {
    public class TareaIntegrationTest : IClassFixture<WebApplicationFactory<Program>> {
        private readonly HttpClient client;

        public TareaIntegrationTest(WebApplicationFactory<Program> factory) {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_CreateTarea_Returns201Created()
        {
            // Arrange
            var nuevaTarea = new CreateTareaDTO {
                CodigoTarea = "TSK-008",
                Titulo = "Integration Test",
                Descripcion = "Verifica creación vía HTTP",
                CriteriosAceptacion = "Debe responder 201",
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(4)
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(nuevaTarea),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PostAsync("/api/v1/task", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}