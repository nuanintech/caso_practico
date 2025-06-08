using FluentValidation;
using task_service.Application.Commands;
using task_service.Shared.DTOs;

namespace task_service.Application.Validation {
    public class AssignTareaCommandValidator : AbstractValidator<AssignTareaCommand> {
        #region Constructores
        public AssignTareaCommandValidator() {
            RuleFor(x => x.assignTareaDTO.UsuarioId).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).Must(id => id != Guid.Empty).WithMessage(MessageValidator.GUIDProporcionadoInvalido).OverridePropertyName("Usuario id");
            RuleFor(x => x.assignTareaDTO.TareaId).NotEmpty().WithMessage(MessageValidator.CampoRequeridoMensaje).Must(id => id != Guid.Empty).WithMessage(MessageValidator.GUIDProporcionadoInvalido).OverridePropertyName("Tarea id");


        }
        #endregion
    }
}