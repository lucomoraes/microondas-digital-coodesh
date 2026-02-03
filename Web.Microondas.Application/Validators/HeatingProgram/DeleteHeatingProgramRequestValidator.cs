using FluentValidation;
using Web.Microondas.Application.UseCases.HeatingProgram.Commands;

namespace Web.Microondas.Application.Validators.HeatingProgram;

public class DeleteHeatingProgramRequestValidator : AbstractValidator<DeleteHeatingProgramRequest>
{
    public DeleteHeatingProgramRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório.");
    }
}