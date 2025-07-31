using Application.Commands.Viagem;
using FluentValidation;

namespace Application.Validators.Viagem;

public class IniciarViagemCommandValidator : AbstractValidator<IniciarViagemCommand>
{
    public IniciarViagemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID da viagem é obrigatório");
    }
} 
