namespace task_service.Domain.Interfaces
{
    public interface IUsuarioService {
        Task<bool> ValidateClientExistenceAsync(Guid usuarioId);
    }
}