using FluentValidation;
using Web.Microondas.Application.UseCases.User.Commands;

namespace Web.Microondas.Application.Validators.User;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.");
        RuleFor(x => x.Firstname)
            .NotEmpty().WithMessage("Primeiro nome é obrigatório.")
            .MaximumLength(100);
        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage("Sobrenome é obrigatório.")
            .MaximumLength(100);
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nome de usuário é obrigatório.")
            .MaximumLength(100);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
    }
}
