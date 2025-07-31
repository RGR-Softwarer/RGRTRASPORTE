using Application.Commands.Passageiro;
using FluentValidation;

namespace Application.Validators.Passageiro;

public class RemoverPassageiroCommandValidator : AbstractValidator<RemoverPassageiroCommand>
{
    public RemoverPassageiroCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID do passageiro é obrigatório");
    }
} 
