namespace client_service.Domain.ValueObjects
{
    public sealed class Edad : IEquatable<Edad> {
        public int Valor { get; }

        // Rango definido como constantes para mayor claridad
        public const int Minima = 15;
        public const int Maxima = 80;

        public Edad(int valor) {
            if (valor < Minima || valor > Maxima)
                throw new ArgumentOutOfRangeException(nameof(valor), $"La edad debe estar entre {Minima} y {Maxima}.");

            Valor = valor;
        }

        public override string ToString() => Valor.ToString();

        // Comparación por valor
        public override bool Equals(object? obj) => Equals(obj as Edad);

        public bool Equals(Edad? other) => other is not null && Valor == other.Valor;

        public override int GetHashCode() => Valor.GetHashCode();

        // Operadores de conversión explícita
        public static explicit operator Edad(int valor) => new Edad(valor);
        // Operadores de conversión implicita
        public static implicit operator int(Edad edad) => edad.Valor;

        // Operadores de igualdad para comodidad
        public static bool operator ==(Edad left, Edad right) => Equals(left, right);
        public static bool operator !=(Edad left, Edad right) => !Equals(left, right);
    }
}
