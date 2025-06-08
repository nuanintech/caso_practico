namespace task_service.Shared.DTOs
{
    public class AssignTareaDTO {
        public Guid TareaId { get; set; }
        public Guid UsuarioId { get; set; }
        public string CodigoTarea { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
    }
}
