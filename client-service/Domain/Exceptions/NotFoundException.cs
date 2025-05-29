namespace client_service.Domain.Exceptions
{
    public class NotFoundException : Exception {
        public NotFoundException(string mensaje)
            : base(mensaje) { }
    }
}
