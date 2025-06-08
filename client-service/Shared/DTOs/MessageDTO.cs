namespace client_service.Shared.DTOs
{
    public class MessageDTO {
        public Guid TareaId { get; set; }
        public Guid UsuarioId { get; set; }
        public string CodigoTarea { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
    }
}
