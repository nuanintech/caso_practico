namespace task_service.Domain.Exceptions {
    public class ConflictException : Exception {
        public ConflictException(string mensaje, Exception? inner = null)
            : base(mensaje, inner) { }
    }
}

