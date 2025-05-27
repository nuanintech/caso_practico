using client_service.Application.Commands;
using client_service.Domain.ValueObjects;
using FluentValidation;

namespace client_service.Application.Validator {
    public class CreateUsuarioCommandValidator : AbstractValidator<CreateUsuarioCommand> {
        public CreateUsuarioCommandValidator() {
            RuleFor(x => x.Identificacion).NotEmpty().WithMessage("La identificación es obligatoria.").Length(10, 23);
            RuleFor(x => x.Nombres).NotEmpty().WithMessage("El nombre es obligatorio.");
            RuleFor(x => x.Apellidos).NotEmpty().WithMessage("El apellido es obligatorio.");
        }
    }
}
