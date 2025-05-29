using client_service.Application.Commands;
using client_service.Shared.DTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace client_service.Shared.Validator {
    public class CreateUsuarioDTOValidator : AbstractValidator<CreateUsuarioDTO> {
        #region Atributos
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion
        #region Constructores
        public CreateUsuarioDTOValidator() {
            RuleFor(x => x.Identificacion).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).Length(10, 23).WithMessage(MessageValidator.LongitudCampoEntreMensaje);
            RuleFor(x => x.Nombres).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje);
            RuleFor(x => x.Apellidos).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje);
            RuleFor(x => x.Email).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).Must(email => EmailRegex.IsMatch(email)).WithMessage("El email no tiene un formato válido.");
            RuleFor(x => x.Edad).InclusiveBetween(15, 70).WithMessage(MessageValidator.AnioCampoEntreMensaje).When(x => x.Edad.HasValue);
        }
        #endregion
    }
}

