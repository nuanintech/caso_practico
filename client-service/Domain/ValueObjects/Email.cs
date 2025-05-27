using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace client_service.Domain.ValueObjects {
    public sealed class Email : IEquatable<Email> {
        public string Valor { get; }

        // Expresión regular compilada y reusable
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public Email(string valor) {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ValidationException("El email no puede estar vacío.");

            if (!EmailRegex.IsMatch(valor))
                throw new ValidationException("El email no tiene un formato válido.");

            Valor = valor.ToLowerInvariant();
        }
        // Método estático para validar un string sin construir el objeto
        public static bool EsValido(string valor) {
            return !string.IsNullOrWhiteSpace(valor) && EmailRegex.IsMatch(valor);
        }

        public override string ToString() => Valor;

        public override bool Equals(object? obj) => Equals(obj as Email);

        public bool Equals(Email? other) => other is not null && Valor == other.Valor;

        public override int GetHashCode() => Valor.GetHashCode();

        public static bool operator ==(Email left, Email right) => Equals(left, right);
        public static bool operator !=(Email left, Email right) => !Equals(left, right);

        public static explicit operator Email(string valor) => new Email(valor);
        public static implicit operator string(Email email) => email.Valor;
    }
}
