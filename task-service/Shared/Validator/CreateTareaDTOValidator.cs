using FluentValidation;
using task_service.Domain.Enum;
using task_service.Shared.DTOs;

namespace task_service.Shared.Validator {
    public class CreateTareaDTOValidator : AbstractValidator<CreateTareaDTO> {
        #region Constructores
        public CreateTareaDTOValidator() {
            RuleFor(x => x.CodigoTarea).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).Length(3, 50).WithMessage(MessageValidator.LongitudCampoEntreMensaje);
            RuleFor(x => x.Titulo).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).MaximumLength(100).WithMessage(MessageValidator.MaximumLengthMensaje);
            RuleFor(x => x.FechaInicio).Must(f => !f.HasValue || f.Value != default).WithMessage(MessageValidator.FechaInvalida);
            RuleFor(x => x.FechaFin).Must(f => !f.HasValue || f.Value != default).WithMessage(MessageValidator.FechaInvalida);
            RuleFor(x => x.EstadoTarea).Must(value => Enum.TryParse<StateTask>(value, true, out _)).WithMessage("EstadoTarea no es válido. Valores permitidos: Backlog, Doing, InReview, Done");
            RuleFor(x => x.UsuarioId).Must(id => id == null || id != Guid.Empty).WithMessage(MessageValidator.GUIDProporcionadoInvalido);
            // Validar fechas si ambas existen
            When(x => x.FechaInicio.HasValue && x.FechaFin.HasValue, () => {
                RuleFor(x => x.FechaFin).GreaterThanOrEqualTo(x => x.FechaInicio!.Value).WithMessage(MessageValidator.FechaMayorIgualOtraFecha);
            });


        }
        #endregion
    }
}
