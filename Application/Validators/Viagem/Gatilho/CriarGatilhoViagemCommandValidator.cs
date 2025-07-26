using Application.Commands.Viagem.Gatilho;
using FluentValidation;

namespace Application.Validators.Viagem.Gatilho;

public class CriarGatilhoViagemCommandValidator : AbstractValidator<CriarGatilhoViagemCommand>
{
    public CriarGatilhoViagemCommandValidator()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória");
        // Adicione outras validações conforme os campos do comando
    }
} 