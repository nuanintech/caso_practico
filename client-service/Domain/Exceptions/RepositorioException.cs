using System;

namespace client_service.Domain.Exceptions
{
    public class RepositorioException : Exception {
        public RepositorioException(string mensaje, Exception? inner = null)
            : base(mensaje, inner) { }
    }
}
