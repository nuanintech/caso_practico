using task_service.Domain.Entities;
using task_service.Domain.Enum;

namespace task_service.Domain.Interfaces {
    public interface ITareaRepositorio {
        Task<Tarea?> GetByIdAsync(Guid id);
        Task<Tarea?> GetByCodigoAsync(string codigo);
        Task<IEnumerable<Tarea>> GetAllAsync();
        Task<Tarea> CreateAsync(Tarea tarea);
        Task UpdateClientAsync(Guid id, Guid usuarioId);
        //Task UpdateAsync(Tarea tarea);
        //Task UpdateStateAsync(Guid id, StateTask nuevoEstado);
        //Task DeleteAsync(Guid id);
    }
}
