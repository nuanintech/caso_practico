namespace task_service.Shared.Validator {
    public class MessageValidator {
        public static string CampoRequeridoMensaje = "El atributo {PropertyName} es requerido";
        public static string LongitudCampoEntreMensaje = "El atributo {PropertyName} debe tener entre {MinLength} y {MaxLength} caracteres.";
        public static string AnioCampoEntreMensaje = "El atributo {PropertyName} debe tener entre {From} y {To} años.";
        public static string MaximumLengthMensaje = "El campo {PropertyName} debe tener menos de {MaxLength} caracteres";
        public static string PrimeraLetraMayusculaMensaje = "El campo {PropertyName} debe comenzar con mayúsculas";
        public static string EmailMensaje = "El campo {PropertyName} debe ser un email válido";
        public static string CampoMayorCero = "El campo {PropertyName} debe ser mayor a cero";
        public static string CampoMayorIgualCero = "El campo {PropertyName} debe ser mayor o igual a cero";
        public static string CampoMayorIgualQue = "El campo {PropertyName} debe ser mayor o igual a cero";
        public static string FechaMayorIgualActual = "El campo {PropertyName} debe ser mayor a igual a la fecha actual";
        public static string FechaMayorIgualOtraFecha = "El campo {PropertyName} debe ser mayor a igual al campo {ComparisonValue}";
        public static string ValorEnumInvalido = "El valor proporcionado al campo {PropertyName} no es válido";
        public const string FechaInvalida = "El campo {PropertyName} debe ser una fecha válida.";
        public const string GUIDProporcionadoInvalido = "El campo {PropertyName} tiene un GUID inválido.";
    }
}
