using client_service.Domain.Entities;

namespace client_service.Domain.Interfaces{
    public interface IUsuarioRepositorio {
        Task<Usuario?> GetByIdAsync(Guid id);
        Task<Usuario?> GetByIdentificationAsync(string identificacion);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario> CreateAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(Guid id);
    }
}
