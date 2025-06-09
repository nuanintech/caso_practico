using FluentValidation;
using task_service.Application.Commands;

namespace task_service.Application.Validation {
    public class UpdateTareaCommandValidator : AbstractValidator<UpdateTareaCommand> {
        #region Constructores
        public UpdateTareaCommandValidator()
        {
            RuleFor(x => x.updateTareaDTO.CodigoTarea).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).Length(3, 50).WithMessage(MessageValidator.LongitudCampoEntreMensaje).OverridePropertyName("Código de tarea");
            RuleFor(x => x.updateTareaDTO.Titulo).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).MaximumLength(100).WithMessage(MessageValidator.MaximumLengthMensaje).OverridePropertyName("Titulo");
            RuleFor(x => x.updateTareaDTO.FechaInicio).Must(f => !f.HasValue || f.Value != default).WithMessage(MessageValidator.FechaInvalida).OverridePropertyName("Fecha inicio");
            RuleFor(x => x.updateTareaDTO.FechaFin).Must(f => !f.HasValue || f.Value != default).WithMessage(MessageValidator.FechaInvalida).OverridePropertyName("Fecha fin");
            // Validar fechas si ambas existen
            When(x => x.updateTareaDTO.FechaInicio.HasValue && x.updateTareaDTO.FechaFin.HasValue, () => {
                RuleFor(x => x.updateTareaDTO.FechaFin).GreaterThanOrEqualTo(x => x.updateTareaDTO.FechaInicio!.Value).WithMessage(MessageValidator.FechaMayorIgualOtraFecha).OverridePropertyName("Fecha fin");
            });
        }
        #endregion
    }
}
