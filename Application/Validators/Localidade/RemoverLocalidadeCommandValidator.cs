using Application.Commands.Localidade;
using FluentValidation;

namespace Application.Validators.Localidade;

public class RemoverLocalidadeCommandValidator : AbstractValidator<RemoverLocalidadeCommand>
{
    public RemoverLocalidadeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID da localidade é obrigatório");
    }
} 
