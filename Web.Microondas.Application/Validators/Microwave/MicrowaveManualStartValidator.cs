using FluentValidation;

namespace Web.Microondas.Application.Validators.Microwave;

public class MicrowaveManualStartValidator : AbstractValidator<(int seconds, int power)>
{
    public MicrowaveManualStartValidator()
    {
        RuleFor(x => x.seconds)
            .InclusiveBetween(1, 120).WithMessage("Tempo deve estar entre 1 segundo e 2 minutos.");
        RuleFor(x => x.power)
            .InclusiveBetween(1, 10).WithMessage("Potência deve estar entre 1 e 10.");
    }
}