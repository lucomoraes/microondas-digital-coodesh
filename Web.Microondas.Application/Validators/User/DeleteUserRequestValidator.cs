using FluentValidation;
using Web.Microondas.Application.UseCases.User.Commands;

namespace Web.Microondas.Application.Validators.User;

public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.");
    }
}
