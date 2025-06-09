using FluentValidation;
using task_service.Application.Commands;
using task_service.Domain.Enum;

namespace task_service.Application.Validation {
    public class UpdateTareaStateCommandValidator : AbstractValidator<UpdateTareaStateCommand> {
        #region Constructores
        public UpdateTareaStateCommandValidator() {
            RuleFor(x => x.updateStateDTO.EstadoTarea).Must(value => Enum.TryParse<StateTask>(value, true, out _)).WithMessage("Estado de tarea no es válido. Valores permitidos: Backlog, Doing, InReview, Done");
        }
        #endregion
    }
}
