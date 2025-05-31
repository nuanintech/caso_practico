using FluentValidation;
using task_service.Application.Commands;
using task_service.Domain.Enum;
using task_service.Shared.DTOs;

namespace task_service.Application.Validation {
    public class CreateTareaCommandValidator : AbstractValidator<CreateTareaCommand> {
        #region Constructores
        public CreateTareaCommandValidator() {
            RuleFor(x => x.createTareaDTO.CodigoTarea).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).Length(3, 50).WithMessage(MessageValidator.LongitudCampoEntreMensaje).OverridePropertyName("Código de tarea");
            RuleFor(x => x.createTareaDTO.Titulo).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).MaximumLength(100).WithMessage(MessageValidator.MaximumLengthMensaje).OverridePropertyName("Titulo");
            RuleFor(x => x.createTareaDTO.FechaInicio).Must(f => !f.HasValue || f.Value != default).WithMessage(MessageValidator.FechaInvalida).OverridePropertyName("Fecha inicio");
            RuleFor(x => x.createTareaDTO.FechaFin).Must(f => !f.HasValue || f.Value != default).WithMessage(MessageValidator.FechaInvalida).OverridePropertyName("Fecha fin");
            RuleFor(x => x.createTareaDTO.EstadoTarea).Must(value => Enum.TryParse<StateTask>(value, true, out _)).WithMessage("Estado de tarea no es válido. Valores permitidos: Backlog, Doing, InReview, Done");
            RuleFor(x => x.createTareaDTO.UsuarioId).Must(id => id == null || id != Guid.Empty).WithMessage(MessageValidator.GUIDProporcionadoInvalido).OverridePropertyName("Usuario id");
            // Validar fechas si ambas existen
            When(x => x.createTareaDTO.FechaInicio.HasValue && x.createTareaDTO.FechaFin.HasValue, () => {
                RuleFor(x => x.createTareaDTO.FechaFin).GreaterThanOrEqualTo(x => x.createTareaDTO.FechaInicio!.Value).WithMessage(MessageValidator.FechaMayorIgualOtraFecha).OverridePropertyName("Fecha fin");
            });


        }
        #endregion
    }
}