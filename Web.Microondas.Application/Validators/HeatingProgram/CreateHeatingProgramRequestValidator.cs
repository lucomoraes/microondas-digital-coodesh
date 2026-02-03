using FluentValidation;
using Web.Microondas.Application.UseCases.HeatingProgram.Commands;
using Web.Microondas.Domain.Interfaces.Repository;

namespace Web.Microondas.Application.Validators.HeatingProgram;

public class CreateHeatingProgramRequestValidator : AbstractValidator<CreateHeatingProgramRequest>
{
    public CreateHeatingProgramRequestValidator(IHeatingProgramRepository repository)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome do programa é obrigatório.")
            .MaximumLength(100);
        RuleFor(x => x.Food)
            .NotEmpty().WithMessage("Alimento é obrigatório.")
            .MaximumLength(200);
        RuleFor(x => x.TimeInSeconds)
            .InclusiveBetween(1, 120).WithMessage("Tempo deve estar entre 1 segundo e 2 minutos.");
        RuleFor(x => x.Power)
            .InclusiveBetween(1, 10).WithMessage("Potência deve estar entre 1 e 10.");
        RuleFor(x => x.Character)
            .NotEqual('.').WithMessage("Caractere '.' é reservado para aquecimento padrão.");
        RuleFor(x => x.Character)
            .MustAsync(async (character, ct) => !await repository.CheckIfExistsAsync(character, ct))
            .WithMessage("Caractere já está em uso por outro programa de aquecimento.");
    }
}