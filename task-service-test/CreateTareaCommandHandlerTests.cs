using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using task_service.Application.Commands;
using task_service.Application.Handlers;
using task_service.Application.Mapping;
using task_service.Domain.Entities;
using task_service.Domain.Exceptions;
using task_service.Domain.Interfaces;
using task_service.Shared.DTOs;

namespace task_service_test {
    public class CreateTareaCommandHandlerTests {
        [Fact]
        public async Task Handle_CreacionCorrectaTarea() {
            // Arrange
            var repoMock = new Mock<ITareaRepositorio>();
            var usuarioServiceMock = new Mock<IUsuarioService>();
            var rabbitMqMock = new Mock<IRabbitMQPublisher>();

            var dto = new CreateTareaDTO {
                CodigoTarea = "TSK-005",
                Titulo = "Crear tarea handler",
                Descripcion = "Test unitario handler",
                CriteriosAceptacion = "Debe pasar",
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(2),
                EstadoTarea = "Backlog",
                UsuarioId = Guid.NewGuid()
            };

            repoMock.Setup(r => r.GetByCodigoAsync(dto.CodigoTarea)).ReturnsAsync((Tarea)null!);
            usuarioServiceMock.Setup(s => s.ValidateClientExistenceAsync(dto.UsuarioId.Value)).ReturnsAsync(true);
            repoMock.Setup(r => r.CreateAsync(It.IsAny<Tarea>())).ReturnsAsync((Tarea t) => t);

            var handler = new CreateTareaHandler(repoMock.Object, usuarioServiceMock.Object, rabbitMqMock.Object);
            var command = new CreateTareaCommand(dto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.CodigoTarea, result.CodigoTarea);
            Assert.Equal(dto.Titulo, result.Titulo);
            rabbitMqMock.Verify(p => p.PublishTaskAssigned(It.IsAny<MessageDTO>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ErrorCodigoDuplicado() {
            // Arrange
            var repoMock = new Mock<ITareaRepositorio>();
            var usuarioServiceMock = new Mock<IUsuarioService>();
            var rabbitMqMock = new Mock<IRabbitMQPublisher>();

            var dto = new CreateTareaDTO { CodigoTarea = "DUP-001" };
            repoMock.Setup(r => r.GetByCodigoAsync(dto.CodigoTarea)).ReturnsAsync(new Tarea {
                CodigoTarea = dto.CodigoTarea,
                Titulo = "Mock Tarea"
            });

            var handler = new CreateTareaHandler(repoMock.Object, usuarioServiceMock.Object, rabbitMqMock.Object);
            var command = new CreateTareaCommand(dto);

            // Act & Assert
            await Assert.ThrowsAsync<ConflictException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ErrorTareaFinalizada()
        {
            // Arrange
            var repoMock = new Mock<ITareaRepositorio>();
            var usuarioServiceMock = new Mock<IUsuarioService>();
            var rabbitMqMock = new Mock<IRabbitMQPublisher>();

            var dto = new CreateTareaDTO
            {
                CodigoTarea = "TSK-007",
                UsuarioId = Guid.NewGuid(),
                EstadoTarea = "Done"
            };

            repoMock.Setup(r => r.GetByCodigoAsync(dto.CodigoTarea)).ReturnsAsync((Tarea)null!);
            usuarioServiceMock.Setup(s => s.ValidateClientExistenceAsync(dto.UsuarioId.Value)).ReturnsAsync(true);

            var handler = new CreateTareaHandler(repoMock.Object, usuarioServiceMock.Object, rabbitMqMock.Object);
            var command = new CreateTareaCommand(dto);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
