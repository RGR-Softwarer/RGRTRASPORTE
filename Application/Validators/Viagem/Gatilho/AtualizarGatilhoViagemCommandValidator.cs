using Application.Commands.Viagem.Gatilho;
using FluentValidation;

namespace Application.Validators.Viagem.Gatilho;

public class AtualizarGatilhoViagemCommandValidator : AbstractValidator<AtualizarGatilhoViagemCommand>
{
    public AtualizarGatilhoViagemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("O ID do gatilho é obrigatório");
        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("A descrição é obrigatória");
        // Adicione outras validações conforme os campos do comando
    }
} 